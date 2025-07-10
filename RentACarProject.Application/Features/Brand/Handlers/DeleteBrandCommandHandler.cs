using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Common;

namespace RentACarProject.Application.Features.Brand.Commands.Handlers
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, ServiceResponse<Guid>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<Guid>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetAsync(x => x.BrandId == request.BrandId);

            if (brand == null)
            {
                return new ServiceResponse<Guid>
                {
                    Success = false,
                    Message = "Marka bulunamadı.",
                    Code = "404"
                };
            }

            await _brandRepository.DeleteAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<Guid>
            {
                Success = true,
                Message = "Marka başarıyla silindi.",
                Data = brand.BrandId
            };
        }
    }
}
