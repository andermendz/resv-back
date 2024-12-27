namespace Reservations.Domain.Entities;

public class Space
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation property
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}