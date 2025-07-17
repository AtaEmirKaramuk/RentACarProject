public class Log
{
    public Guid LogId { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string IPAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string LogLevel { get; set; } = "Info"; // Varsayılan Info
    public string? ExceptionMessage { get; set; }
    public int? ElapsedMilliseconds { get; set; }
}
