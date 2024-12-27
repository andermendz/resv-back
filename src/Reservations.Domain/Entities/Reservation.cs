using Reservations.Domain.Enums;

namespace Reservations.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SpaceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ReservationStatus Status { get; set; }
    public virtual User? User { get; set; }
    public virtual Space? Space { get; set; }

    public bool IsValidDuration()
    {
        var duration = EndTime - StartTime;
        return duration >= TimeSpan.FromMinutes(30) && duration <= TimeSpan.FromHours(8);
    }
} 