﻿namespace RentACarProject.Application.DTOs.Model
{
    public class CreateModelDto
    {
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }
    }
}
