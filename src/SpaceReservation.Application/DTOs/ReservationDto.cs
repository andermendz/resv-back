namespace SpaceReservation.Application.DTOs;

public record ReservationDto(
    int Id,
    int SpaceId,
    string SpaceName,
    string Cedula,
    DateTime StartTime,
    DateTime EndTime
); 