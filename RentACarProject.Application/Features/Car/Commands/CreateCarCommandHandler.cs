using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, ServiceResponse<CarResponseDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCarCommandHandler(ICarRepository carRepository, IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<CarResponseDto>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.Query()
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.ModelId == request.ModelId, cancellationToken);

            if (model == null)
            {
                throw new BusinessException("Model bulunamadı.");
            }

            var newCar = new Domain.Entities.Car
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

            var dto = new CarResponseDto
            {
                Id = newCar.CarId,
                BrandName = model.Brand.Name,
                ModelName = model.Name,
                Year = newCar.Year,
                Plate = newCar.Plate,
                DailyPrice = newCar.DailyPrice,
                Description = newCar.Description,
                Status = newCar.Status
            };

            return new ServiceResponse<CarResponseDto>
            {
                Success = true,
                Message = $"Araç \"{dto.Plate}\" başarıyla eklendi.",
                Data = dto
            };
        }
    }
}
