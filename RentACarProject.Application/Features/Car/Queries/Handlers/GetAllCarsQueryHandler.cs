using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries.Handlers
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
            var cars = await _carRepository.Query()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .ToListAsync();

            var result = cars.Select(c => new CarResponseDto
            {
                CarId = c.CarId,
                BrandName = c.Model.Brand.Name,
                ModelName = c.Model.Name,
                Year = c.Year,
                Plate = c.Plate,
                DailyPrice = c.DailyPrice,
                Description = c.Description,
                Status = c.Status
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
