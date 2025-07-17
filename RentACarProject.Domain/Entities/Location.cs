using RentACarProject.Domain.Common;
using RentACarProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class Location : BaseEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string City { get; set; }

    public ICollection<Reservation> PickupReservations { get; set; } = new List<Reservation>();
    public ICollection<Reservation> DropoffReservations { get; set; } = new List<Reservation>();
}
