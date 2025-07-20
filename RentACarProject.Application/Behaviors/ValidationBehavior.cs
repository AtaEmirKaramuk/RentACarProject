using FluentValidation;
using MediatR;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var message = string.Join(" | ", failures.Select(e => e.ErrorMessage));

                    // ServiceResponse<T> türünde bir response oluştur
                    var responseType = typeof(TResponse);
                    var serviceResponse = Activator.CreateInstance(responseType);

                    serviceResponse!.GetType().GetProperty("Success")?.SetValue(serviceResponse, false);
                    serviceResponse.GetType().GetProperty("Message")?.SetValue(serviceResponse, message);
                    serviceResponse.GetType().GetProperty("Code")?.SetValue(serviceResponse, "VALIDATION_ERROR");

                    return (TResponse)serviceResponse!;
                }
            }

            return await next();
        }
    }
}
