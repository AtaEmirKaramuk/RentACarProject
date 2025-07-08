using RentACarProject.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentACarProject.Domain.Entities
{
    public class Model : BaseEntity
    {
        public Guid ModelId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }

        public Brand Brand { get; set; } = null!;
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
