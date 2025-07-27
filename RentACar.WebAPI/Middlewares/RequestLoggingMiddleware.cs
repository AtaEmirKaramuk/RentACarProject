using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RentACarProject.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // 🟠 Request Body'yi oku
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            // 🟠 Response Body'yi geçici belleğe al
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                // İşlemi yürüt
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                try
                {
                    // Response gövdesini oku
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    // 🟢 Serilog'a tüm custom kolonları gönder
                    LogContext.PushProperty("Path", context.Request.Path);
                    LogContext.PushProperty("Method", context.Request.Method);
                    LogContext.PushProperty("StatusCode", context.Response.StatusCode);
                    LogContext.PushProperty("RequestBody", requestBody);
                    LogContext.PushProperty("ResponseBody", responseText);
                    LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress?.ToString());
                    LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString());
                    LogContext.PushProperty("ResponseTimeMs", stopwatch.ElapsedMilliseconds);

                    var userId = context.User?.Claims?.FirstOrDefault(c => c.Type == "uid")?.Value;
                    if (Guid.TryParse(userId, out Guid parsedUserId))
                        LogContext.PushProperty("UserId", parsedUserId);
                    else
                        LogContext.PushProperty("UserId", Guid.Empty);

                    LogContext.PushProperty("TraceId", context.TraceIdentifier);

                    // 🟢 Bilgi logunu yaz (Veritabanı kolonları dolacak)
                    Log.Information("HTTP {Method} {Path} responded {StatusCode} in {ResponseTimeMs} ms");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Log kaydı alınırken hata oluştu.");
                }

                // 🟠 Orijinal response stream'ine geri dön
                try
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                catch (Exception copyEx)
                {
                    Log.Error(copyEx, "Yanıt gövdesi orijinale kopyalanırken hata oluştu.");
                }

                context.Response.Body = originalBodyStream;
            }
        }
    }
}
