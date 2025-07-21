using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.ProfileUpdate;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.ProfileUpdate.Commands
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ServiceResponse<UpdateProfileDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProfileCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<UpdateProfileDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                throw new BusinessException("Kullanıcı bilgisi alınamadı.");

            var user = await _userRepository.Query()
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user == null)
                throw new NotFoundException("Kullanıcı bulunamadı.");

            if (user.Customer == null)
                throw new NotFoundException("Müşteri bilgisi bulunamadı.");

            var customer = user.Customer;

            // Email benzersizlik kontrolü
            if (!string.IsNullOrWhiteSpace(request.Profile.Email) && request.Profile.Email != customer.Email)
            {
                var emailExists = await _userRepository.ExistsAsync(u =>
                    u.Email == request.Profile.Email && u.UserId != user.UserId);

                if (emailExists)
                    throw new BusinessException("Bu email adresi zaten kullanımda.");

                customer.Email = request.Profile.Email;
                user.Email = request.Profile.Email; // User tablosundaki email'i de güncelle
            }

            // Telefon benzersizlik kontrolü
            if (!string.IsNullOrWhiteSpace(request.Profile.Phone) && request.Profile.Phone != customer.Phone)
            {
                var phoneExists = await _userRepository.Query()
                    .Include(u => u.Customer)
                    .AnyAsync(u => u.Customer != null &&
                                   u.Customer.Phone == request.Profile.Phone &&
                                   u.UserId != user.UserId);

                if (phoneExists)
                    throw new BusinessException("Bu telefon numarası zaten kullanımda.");

                customer.Phone = request.Profile.Phone;
            }

            if (!string.IsNullOrWhiteSpace(request.Profile.FirstName))
                customer.FirstName = request.Profile.FirstName;

            if (!string.IsNullOrWhiteSpace(request.Profile.LastName))
                customer.LastName = request.Profile.LastName;

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<UpdateProfileDto>
            {
                Success = true,
                Message = "Profil güncellendi.",
                Data = request.Profile
            };
        }
    }
}
