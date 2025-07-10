using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.Entities;

namespace RentACarProject.Application.Features.Car.Commands.Handlers
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, ServiceResponse<Guid>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var newCar = new RentACarProject.Domain.Entities.Car
            {
                CarId = Guid.NewGuid(),
                ModelId = request.ModelId,
                Year = request.Year,
                Plate = request.Plate,
                DailyPrice = request.DailyPrice,
                Description = request.Description,
                Status = true
            };

            await _carRepository.AddAsync(newCar);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Araç başarıyla eklendi.",
                Data = newCar.CarId
            };
        }
    }
}
