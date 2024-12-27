using MediatR;
using Reservations.Application.DTOs;
using Reservations.Domain.Interfaces;

namespace Reservations.Application.Queries;

public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository _repository;

    public GetReservationsQueryHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _repository.GetAllAsync();
        
        // Apply filters
        var filtered = reservations.Where(r =>
            (!request.StartDate.HasValue || r.StartTime >= request.StartDate) &&
            (!request.EndDate.HasValue || r.EndTime <= request.EndDate) &&
            (!request.UserId.HasValue || r.UserId == request.UserId) &&
            (!request.SpaceId.HasValue || r.SpaceId == request.SpaceId));

        // Map to DTOs
        return filtered.Select(r => new ReservationDto
        {
            Id = r.Id,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Notes = r.Notes,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            UserId = r.UserId,
            UserName = r.User?.Name ?? string.Empty,
            SpaceId = r.SpaceId,
            SpaceName = r.Space?.Name ?? string.Empty
        });
    }
} 