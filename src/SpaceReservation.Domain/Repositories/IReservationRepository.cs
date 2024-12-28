using SpaceReservation.Domain.Entities;

namespace SpaceReservation.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(int id);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<IEnumerable<Reservation>> GetBySpaceIdAsync(int spaceId);
    Task<IEnumerable<Reservation>> GetByCedulaAsync(string cedula);
    Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(int spaceId, DateTime startTime, DateTime endTime);
    Task<IEnumerable<Reservation>> GetUserOverlappingReservationsAsync(string cedula, DateTime startTime, DateTime endTime);
    Task<Reservation> AddAsync(Reservation reservation);
    Task DeleteAsync(Reservation reservation);
} 