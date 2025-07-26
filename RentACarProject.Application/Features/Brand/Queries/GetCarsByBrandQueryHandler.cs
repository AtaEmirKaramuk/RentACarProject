using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetCarsByBrandQueryHandler : IRequestHandler<GetCarsByBrandQuery, ServiceResponse<List<CarResponseDto>>>
    {
        private readonly ICarRepository _carRepository;

        public GetCarsByBrandQueryHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<ServiceResponse<List<CarResponseDto>>> Handle(GetCarsByBrandQuery request, CancellationToken cancellationToken)
        {
            var cars = await _carRepository.Query()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(c => c.Model.Brand.Name.ToLower() == request.BrandName.ToLower())
                .ToListAsync(cancellationToken);

            var result = cars.Select(c => new CarResponseDto
            {
                Id = c.Id,
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
