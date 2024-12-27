using MediatR;
using Reservations.Domain.Interfaces;
using Reservations.Domain.Entities;
using Reservations.Domain.Exceptions;
using Reservations.Domain.Enums;

namespace Reservations.Application.Commands;

public record CancelReservationCommand(Guid ReservationId) : IRequest<Unit>;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Unit>
{
    private readonly IReservationRepository _repository;

    public CancelReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.ReservationId);
        
        if (reservation == null)
            throw new NotFoundException("Reservation not found");

        if (reservation.Status == ReservationStatus.Cancelled)
            throw new BusinessException("Reservation is already cancelled");

        reservation.Status = ReservationStatus.Cancelled;
        reservation.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(reservation);
        return Unit.Value;
    }
} 