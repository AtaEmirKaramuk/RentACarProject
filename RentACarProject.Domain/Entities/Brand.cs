using RentACarProject.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentACarProject.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public Guid BrandId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
