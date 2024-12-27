using MediatR;
using Reservations.Application.DTOs;

namespace Reservations.Application.Queries;

public record GetReservationsQuery(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? UserId = null,
    Guid? SpaceId = null
) : IRequest<IEnumerable<ReservationDto>>;