﻿namespace RentACarProject.Domain.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Role { get; set; }
    }
}
