using Microsoft.EntityFrameworkCore;
using SpaceReservation.Domain.Entities;
using SpaceReservation.Domain.Repositories;
using SpaceReservation.Infrastructure.Persistence;

namespace SpaceReservation.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetBySpaceIdAsync(int spaceId)
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .Where(r => r.SpaceId == spaceId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByCedulaAsync(string cedula)
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .Where(r => r.Cedula == cedula)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Reservations.Include(r => r.Space).AsQueryable();

        if (startDate.HasValue)
            query = query.Where(r => r.StartTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(r => r.EndTime <= endDate.Value);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(
        int spaceId, DateTime startTime, DateTime endTime)
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .Where(r => r.SpaceId == spaceId &&
                       r.StartTime < endTime &&
                       r.EndTime > startTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetUserOverlappingReservationsAsync(
        string cedula, DateTime startTime, DateTime endTime)
    {
        return await _context.Reservations
            .Include(r => r.Space)
            .Where(r => r.Cedula == cedula &&
                       r.StartTime < endTime &&
                       r.EndTime > startTime)
            .ToListAsync();
    }

    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task DeleteAsync(Reservation reservation)
    {
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
    }
} 