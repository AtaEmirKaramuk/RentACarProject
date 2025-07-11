using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;

namespace RentACarProject.Application.Features.Car.Commands.Handlers
{
    public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ServiceResponse<DeletedCarDto>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCarCommandHandler(ICarRepository carRepository, IModelRepository modelRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _modelRepository = modelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<DeletedCarDto>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetAsync(c => c.CarId == request.CarId);
            if (car == null)
            {
                return new ServiceResponse<DeletedCarDto>
                {
                    Success = false,
                    Message = "Araç bulunamadı.",
                    Code = "404"
                };
            }

            var model = await _modelRepository.Query()
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.ModelId == car.ModelId, cancellationToken);

            if (model == null)
            {
                return new ServiceResponse<DeletedCarDto>
                {
                    Success = false,
                    Message = "Araç modeli bulunamadı.",
                    Code = "404"
                };
            }

            await _carRepository.DeleteAsync(car);
            await _unitOfWork.SaveChangesAsync();

            var dto = new DeletedCarDto
            {
                CarId = car.CarId,
                Plate = car.Plate,
                ModelName = model.Name,
                BrandName = model.Brand.Name
            };

            return new ServiceResponse<DeletedCarDto>
            {
                Success = true,
                Message = $"Araç \"{dto.Plate}\" başarıyla silindi.",
                Data = dto
            };
        }
    }
}
