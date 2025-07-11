﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Queries.Handlers
{
    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, ServiceResponse<CarResponseDto>>
    {
        private readonly ICarRepository _carRepository;

        public GetCarByIdQueryHandler(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<ServiceResponse<CarResponseDto>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.Query()
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.CarId == request.CarId);

            if (car == null)
            {
                return new ServiceResponse<CarResponseDto>
                {
                    Success = false,
                    Message = "Araç bulunamadı.",
                    Code = "404"
                };
            }

            var dto = new CarResponseDto
            {
                CarId = car.CarId,
                BrandName = car.Model.Brand.Name,
                ModelName = car.Model.Name,
                Year = car.Year,
                Plate = car.Plate,
                DailyPrice = car.DailyPrice,
                Description = car.Description,
                Status = car.Status
            };

            return new ServiceResponse<CarResponseDto>
            {
                Success = true,
                Message = "Araç bulundu.",
                Data = dto
            };
        }
    }
}
