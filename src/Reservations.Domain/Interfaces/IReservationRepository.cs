using Reservations.Domain.Entities;

namespace Reservations.Domain.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Reservation>> GetOverlappingReservations(Guid spaceId, DateTime start, DateTime end);
    Task<IEnumerable<Reservation>> GetUserReservations(Guid userId, DateTime start, DateTime end);
    Task<IEnumerable<Reservation>> GetFilteredReservations(Guid? spaceId, Guid? userId, DateTime? startDate, DateTime? endDate);
}
