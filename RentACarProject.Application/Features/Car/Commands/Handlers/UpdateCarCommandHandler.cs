using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Car.Commands.Handlers
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ServiceResponse<Guid>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCarCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
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

            car.ModelId = request.ModelId;
            car.Year = request.Year;
            car.Plate = request.Plate;
            car.DailyPrice = request.DailyPrice;
            car.Description = request.Description;
            car.Status = request.Status;

            await _carRepository.UpdateAsync(car);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Araç başarıyla güncellendi.",
                Data = car.CarId
            };
        }
    }
}
