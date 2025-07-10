using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Car.Commands.Handlers
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ServiceResponse<Guid>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCarCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetAsync(c => c.CarId == request.CarId);
            if (car == null)
            {
                return new ServiceResponse<Guid>
                {
                    Success = false,
                    Message = "Araç bulunamadı.",
                    Code = "404"
                };
            }

            await _carRepository.DeleteAsync(car);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Araç başarıyla silindi.",
                Data = car.CarId
            };
        }
    }
}
