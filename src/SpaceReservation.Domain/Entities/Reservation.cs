using SpaceReservation.Domain.Common;
using SpaceReservation.Domain.Exceptions;

namespace SpaceReservation.Domain.Entities;

public class Reservation : Entity
{
    public int SpaceId { get; private set; }
    public Space Space { get; private set; } = null!;
    public string Cedula { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    private Reservation() { } // For EF Core

    public Reservation(Space space, string cedula, DateTime startTime, DateTime endTime)
    {
        if (string.IsNullOrWhiteSpace(cedula))
            throw new ArgumentException("Cedula cannot be empty", nameof(cedula));

        if (startTime >= endTime)
            throw new DomainException("End time must be after start time");

        if (startTime < DateTime.UtcNow)
            throw new DomainException("Cannot create reservations in the past");

        if ((endTime - startTime).TotalHours > 8)
            throw new DomainException("Reservation cannot be longer than 8 hours");

        Space = space ?? throw new ArgumentNullException(nameof(space));
        SpaceId = space.Id;
        Cedula = cedula;
        StartTime = startTime;
        EndTime = endTime;
    }

    public bool OverlapsWith(Reservation other)
    {
        return SpaceId == other.SpaceId && 
               StartTime < other.EndTime && 
               EndTime > other.StartTime;
    }

    public bool IsForSameUser(string cedula)
    {
        return Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase);
    }
} 