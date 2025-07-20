using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;
using RentACarProject.Application.Features.Car.Queries;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ServiceResponse<List<CarResponseDto>>>
    {
        private readonly ICarRepository _carRepository;

        public GetAllCarsQueryHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<ServiceResponse<List<CarResponseDto>>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            var carsQuery = _carRepository.Query()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .AsQueryable();

            if (request.BrandId.HasValue)
                carsQuery = carsQuery.Where(c => c.Model.BrandId == request.BrandId.Value);

            if (request.ModelId.HasValue)
                carsQuery = carsQuery.Where(c => c.ModelId == request.ModelId.Value);

            if (!string.IsNullOrWhiteSpace(request.BrandName))
                carsQuery = carsQuery.Where(c => c.Model.Brand.Name.ToLower() == request.BrandName.ToLower());

            if (!string.IsNullOrWhiteSpace(request.ModelName))
                carsQuery = carsQuery.Where(c => c.Model.Name.ToLower() == request.ModelName.ToLower());

            if (request.MinYear.HasValue)
                carsQuery = carsQuery.Where(c => c.Year >= request.MinYear.Value);

            if (request.MaxYear.HasValue)
                carsQuery = carsQuery.Where(c => c.Year <= request.MaxYear.Value);

            if (request.MinPrice.HasValue)
                carsQuery = carsQuery.Where(c => c.DailyPrice >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                carsQuery = carsQuery.Where(c => c.DailyPrice <= request.MaxPrice.Value);

            if (request.Status.HasValue)
                carsQuery = carsQuery.Where(c => c.Status == request.Status.Value);

            // ✅ Yeni filtreler
            if (request.VehicleClass.HasValue)
                carsQuery = carsQuery.Where(c => c.VehicleClass == request.VehicleClass.Value);

            if (request.FuelType.HasValue)
                carsQuery = carsQuery.Where(c => c.FuelType == request.FuelType.Value);

            if (request.TransmissionType.HasValue)
                carsQuery = carsQuery.Where(c => c.TransmissionType == request.TransmissionType.Value);

            var cars = await carsQuery.ToListAsync(cancellationToken);

            var result = cars.Select(c => new CarResponseDto
            {
                Id = c.CarId,
                BrandName = c.Model.Brand.Name,
                ModelName = c.Model.Name,
                Year = c.Year,
                Plate = c.Plate,
                DailyPrice = c.DailyPrice,
                Description = c.Description,
                Status = c.Status,
                VehicleClass = c.VehicleClass,
                FuelType = c.FuelType,
                TransmissionType = c.TransmissionType
            }).ToList();

            return new ServiceResponse<List<CarResponseDto>>
            {
                Success = true,
                Message = "Araçlar listelendi.",
                Data = result
            };
        }
    }
}
