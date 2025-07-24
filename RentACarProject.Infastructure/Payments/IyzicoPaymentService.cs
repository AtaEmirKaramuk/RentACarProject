using Microsoft.Extensions.Options;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Infrastructure.Configurations;
using RentACarProject.Application.DTOs.Payment;
using RentACarProject.Application.Exceptions;
using RentACarProject.Domain.Entities;
using RentACarProject.Domain.Enums;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RentACarProject.Infrastructure.Services.Payments
{
    public class IyzicoPaymentService : IPaymentStrategyService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly HttpClient _httpClient;
        private readonly IyzicoSettings _iyzicoSettings;

        public IyzicoPaymentService(
            IPaymentRepository paymentRepository,
            IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpClientFactory httpClientFactory,
            IOptions<IyzicoSettings> iyzicoOptions)
        {
            _paymentRepository = paymentRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _httpClient = httpClientFactory.CreateClient("iyzico");
            _iyzicoSettings = iyzicoOptions.Value;

            // Base URL set ediliyorsa burada yapılabilir
            _httpClient.BaseAddress = new Uri(_iyzicoSettings.BaseUrl);
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(CreatePaymentDto dto)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.IsDeleted)
                throw new NotFoundException("Rezervasyon bulunamadı.");

            var iyzicoRequest = new
            {
                price = dto.Amount,
                currency = "TRY",
                cardHolderName = dto.CardHolderName,
                cardNumber = dto.CardNumber,
                expireMonth = dto.ExpireMonth,
                expireYear = dto.ExpireYear,
                cvc = dto.Cvc,
                externalId = Guid.NewGuid().ToString(),
                apiKey = _iyzicoSettings.ApiKey,
                secretKey = _iyzicoSettings.SecretKey
            };

            var content = new StringContent(JsonSerializer.Serialize(iyzicoRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/payments", content);
            var resultContent = await response.Content.ReadAsStringAsync();

            var isSuccess = response.IsSuccessStatusCode;

            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(),
                ReservationId = dto.ReservationId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                Type = PaymentType.CreditCard,
                Status = isSuccess ? PaymentStatus.Completed : PaymentStatus.Failed,
                TransactionId = isSuccess ? iyzicoRequest.externalId : null,
                CreatedByUserId = _currentUserService.UserId
            };

            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                ReservationId = payment.ReservationId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Type = payment.Type,
                Status = payment.Status,
                TransactionId = payment.TransactionId
            };
        }
    }
}
