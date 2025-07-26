using RentACarProject.Domain.Common;

namespace RentACarProject.Domain.Entities
{
    public class Model : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid BrandId { get; set; }

        public Brand Brand { get; set; } = null!;
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
