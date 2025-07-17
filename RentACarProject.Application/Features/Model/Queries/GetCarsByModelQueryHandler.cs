using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Domain.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries
{
    public class GetCarsByModelQueryHandler : IRequestHandler<GetCarsByModelQuery, ServiceResponse<List<CarResponseDto>>>
    {
        private readonly ICarRepository _carRepository;

        public GetCarsByModelQueryHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<ServiceResponse<List<CarResponseDto>>> Handle(GetCarsByModelQuery request, CancellationToken cancellationToken)
        {
            var cars = await _carRepository.Query()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .Where(c => c.Model.Name.ToLower() == request.ModelName.ToLower())
                .ToListAsync(cancellationToken);

            var result = cars.Select(c => new CarResponseDto
            {
                Id = c.CarId,
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
