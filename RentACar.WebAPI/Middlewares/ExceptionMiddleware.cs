using System.Net;
using System.Text.Json;
using FluentValidation;
using RentACarProject.Application.Common;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyiniz.";
                var code = "UNEXPECTED_ERROR";

                switch (ex)
                {
                    case BusinessException businessEx when businessEx.Message.Contains("bulunamadı"):
                        statusCode = (int)HttpStatusCode.NotFound;
                        message = businessEx.Message;
                        code = "NOT_FOUND";
                        break;

                    case BusinessException businessEx:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        message = businessEx.Message;
                        code = "BUSINESS_ERROR";
                        break;

                    case ValidationException validationEx:
                        statusCode = (int)HttpStatusCode.UnprocessableEntity;
                        message = string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage));
                        code = "VALIDATION_ERROR";
                        break;

                    case UnauthorizedAccessException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Yetkisiz erişim. Lütfen giriş yapınız.";
                        code = "UNAUTHORIZED";
                        break;

                    case ForbiddenAccessException forbiddenAccessEx:
                        statusCode = (int)HttpStatusCode.Forbidden;
                        message = "Bu kaynağa erişim izniniz yok.";
                        code = "FORBIDDEN";
                        break;

                    case NotFoundException notFoundEx:
                        statusCode = (int)HttpStatusCode.NotFound;
                        message = notFoundEx.Message;
                        code = "NOT_FOUND";
                        break;
                }

                _logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new ServiceResponse<string>
                {
                    Success = false,
                    Message = message,
                    Code = code,
                    Data = null
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Erişim engellendi.") : base(message) { }
    }
}
