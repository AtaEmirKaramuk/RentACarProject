using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.ProfileUpdate;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.Application.Features.ProfileUpdate.Queries
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, ServiceResponse<UpdateProfileDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyProfileQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResponse<UpdateProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException("Kullanıcı bilgisi alınamadı.");

            var user = await _userRepository.Query()
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user == null)
                throw new NotFoundException("Kullanıcı bulunamadı.");

            if (user.Customer == null)
                throw new NotFoundException("Müşteri bilgisi bulunamadı.");

            var profileDto = new UpdateProfileDto
            {
                FirstName = user.Customer.FirstName,
                LastName = user.Customer.LastName,
                Email = user.Customer.Email,
                Phone = user.Customer.Phone
            };

            return new ServiceResponse<UpdateProfileDto>
            {
                Success = true,
                Data = profileDto
            };
        }
    }
}
