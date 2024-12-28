using SpaceReservation.Application.DTOs;

namespace SpaceReservation.Application.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<ReservationDto?> GetReservationByIdAsync(int id);
    Task<IEnumerable<ReservationDto>> GetReservationsBySpaceIdAsync(int spaceId);
    Task<IEnumerable<ReservationDto>> GetReservationsByCedulaAsync(string cedula);
    Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime? startDate, DateTime? endDate);
    Task<ReservationDto> CreateReservationAsync(int spaceId, string cedula, DateTime startTime, DateTime endTime);
    Task DeleteReservationAsync(int id);
} 