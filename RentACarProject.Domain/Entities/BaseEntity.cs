namespace RentACarProject.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public Guid? CreatedByUserId { get; set; } // Artık GUID
        public Guid? ModifiedByUserId { get; set; } // Artık GUID
    }
}
