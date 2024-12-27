public record ReservationCreatedEvent(
    Guid ReservationId,
    Guid UserId,
    Guid SpaceId,
    DateTime StartTime,
    DateTime EndTime
);

public record ReservationCancelledEvent(
    Guid ReservationId,
    Guid UserId,
    DateTime CancelledAt
); 