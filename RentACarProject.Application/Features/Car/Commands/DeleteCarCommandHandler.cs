using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Car;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.Car.Commands
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
            var car = await _carRepository.GetAsync(c => c.Id == request.Id);
            if (car == null)
            {
                throw new BusinessException("Araç bulunamadı.");
            }

            var model = await _modelRepository.Query()
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == car.ModelId, cancellationToken);

            if (model == null)
            {
                throw new BusinessException("Araç modeli bulunamadı.");
            }

            await _carRepository.DeleteAsync(car);
            await _unitOfWork.SaveChangesAsync();

            var dto = new DeletedCarDto
            {
                Id = car.Id,
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
