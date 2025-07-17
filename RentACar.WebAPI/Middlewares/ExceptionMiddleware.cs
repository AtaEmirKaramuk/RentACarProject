using System.Net;
using System.Text.Json;
using FluentValidation;
using RentACarProject.Application.Exceptions;

namespace RentACarProject.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyiniz.";

                switch (ex)
                {
                    // 404: "bulunamadı" içeren business hatalar
                    case BusinessException businessEx when businessEx.Message.Contains("bulunamadı"):
                        statusCode = (int)HttpStatusCode.NotFound;
                        message = businessEx.Message;
                        break;

                    // 400: Diğer business hatalar
                    case BusinessException businessEx:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        message = businessEx.Message;
                        break;

                    // 422: Validation hataları
                    case ValidationException validationEx:
                        statusCode = (int)HttpStatusCode.UnprocessableEntity;
                        message = string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage));
                        break;

                    // 401: Yetkilendirme hatası (opsiyonel özel durumlar için)
                    case UnauthorizedAccessException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Yetkisiz erişim. Lütfen giriş yapınız.";
                        break;

                    // 403: Yasaklı erişim (opsiyonel özel durumlar için)
                    case ForbiddenAccessException:
                        statusCode = (int)HttpStatusCode.Forbidden;
                        message = "Bu kaynağa erişim izniniz yok.";
                        break;
                }

                _logger.LogError(ex, "Bir hata oluştu: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    success = false,
                    message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

    // ⚡ 403 için custom exception sınıfı
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message = "Erişim engellendi.") : base(message) { }
    }
}
