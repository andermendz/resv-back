using MediatR;
using Reservations.Domain.Interfaces;
using Reservations.Domain.Entities;
using Reservations.Domain.Exceptions;
using Reservations.Domain.Enums;

namespace Reservations.Application.Commands;

public record CreateReservationCommand(
    Guid UserId,
    Guid SpaceId,
    DateTime StartTime,
    DateTime EndTime,
    string Notes
) : IRequest<Guid>;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _repository;

    public CreateReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        // Check for existing reservations in the same time slot
        var existingReservations = await _repository.GetOverlappingReservations(
            request.SpaceId,
            request.StartTime,
            request.EndTime
        );

        if (existingReservations.Any())
        {
            throw new BusinessException("There is already a reservation for this space at the requested time.");
        }

        // Check if user has other reservations at the same time
        var userReservations = await _repository.GetUserReservations(
            request.UserId,
            request.StartTime,
            request.EndTime
        );

        if (userReservations.Any())
        {
            throw new BusinessException("User already has a reservation at the requested time.");
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            SpaceId = request.SpaceId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            Status = ReservationStatus.Active
        };

        if (!reservation.IsValidDuration())
        {
            throw new BusinessException("Reservation duration must be between 30 minutes and 8 hours.");
        }

        await _repository.CreateAsync(reservation);
        return reservation.Id;
    }
} 