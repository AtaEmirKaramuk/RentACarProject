using RentACarProject.Domain.Common;
using System;

namespace RentACarProject.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = "Info";
        public string Message { get; set; } = string.Empty;

        public string? Path { get; set; }
        public string? Method { get; set; }
        public int? StatusCode { get; set; }

        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public int? ResponseTimeMs { get; set; }

        public string? UserId { get; set; }
        public string? TraceId { get; set; }

        
    }
}
