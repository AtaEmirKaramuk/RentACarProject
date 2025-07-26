using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Enums;

namespace RentACarProject.Application.Features.Car.Commands
{
    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ServiceResponse<CarResponseDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCarCommandHandler(ICarRepository carRepository, IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<CarResponseDto>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetAsync(c => c.Id == request.CarId);
            if (car == null)
                throw new BusinessException("Araç bulunamadı.");

            var model = await _modelRepository.Query()
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == request.ModelId, cancellationToken);

            if (model == null)
                throw new BusinessException("Model bulunamadı.");

            // ✅ Aynı plaka başka bir araçta var mı kontrolü
            var carWithSamePlate = await _carRepository.GetAsync(c =>
                c.Plate.ToLower() == request.Plate.ToLower() &&
                c.Id != request.CarId);
            if (carWithSamePlate != null)
                throw new BusinessException($"\"{request.Plate}\" plakalı başka bir araç zaten mevcut.");

            car.ModelId = request.ModelId;
            car.Year = request.Year;
            car.Plate = request.Plate;
            car.DailyPrice = request.DailyPrice;
            car.Description = request.Description;
            car.Status = request.Status;
            car.VehicleClass = request.VehicleClass;
            car.FuelType = request.FuelType;
            car.TransmissionType = request.TransmissionType;

            await _carRepository.UpdateAsync(car);
            await _unitOfWork.SaveChangesAsync();

            var dto = new CarResponseDto
            {
                Id = car.Id,
                BrandName = model.Brand.Name,
                ModelName = model.Name,
                Year = car.Year,
                Plate = car.Plate,
                DailyPrice = car.DailyPrice,
                Description = car.Description,
                Status = car.Status,
                VehicleClass = car.VehicleClass,
                FuelType = car.FuelType,
                TransmissionType = car.TransmissionType
            };

            return new ServiceResponse<CarResponseDto>
            {
                Success = true,
                Message = $"Araç \"{dto.Plate}\" başarıyla güncellendi.",
                Data = dto
            };
        }
    }
}
