using RentACarProject.Domain.Common;

namespace RentACarProject.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
