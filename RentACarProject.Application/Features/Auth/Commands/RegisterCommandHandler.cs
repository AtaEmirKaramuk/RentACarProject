using MediatR;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Common;
using RentACarProject.Application.DTOs.Auth;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace RentACarProject.Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ServiceResponse<RegisterResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Kullanıcı adı kontrolü
                var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);
                if (existingUser != null)
                {
                    throw new BusinessException("Bu kullanıcı adı zaten kullanılıyor.");
                }

                // Email kontrolü
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                    if (existingEmail != null)
                    {
                        throw new BusinessException("Bu email zaten kullanılıyor.");
                    }
                }

                // Şifre hash
                var hashedPassword = HashPassword(request.Password);

                // User oluştur
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = request.UserName,
                    PasswordHash = hashedPassword,
                    Email = request.Email,
                    Role = UserRole.User
                };
                await _userRepository.AddAsync(newUser);

                // Customer oluştur
                var newCustomer = new Customer
                {
                    UserId = newUser.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Email = request.Email
                };
                await _customerRepository.AddAsync(newCustomer);

                // DB kayıt
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new ServiceResponse<RegisterResponseDto>
                {
                    Success = true,
                    Message = "Kayıt başarılı.",
                    Code = "200",
                    Data = new RegisterResponseDto
                    {
                        UserId = newUser.UserId,
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        Role = newUser.Role.ToString()
                    }
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw; // ExceptionMiddleware yakalar
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
