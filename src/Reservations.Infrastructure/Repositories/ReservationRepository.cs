using Microsoft.EntityFrameworkCore;
using Reservations.Domain.Entities;
using Reservations.Domain.Interfaces;
using Reservations.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reservations.Infrastructure.Data;

namespace Reservations.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ReservationDbContext _context;

    public ReservationRepository(ReservationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Space)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Space)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Guid> CreateAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return reservation.Id;
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var reservation = await GetByIdAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Reservation>> GetOverlappingReservations(Guid spaceId, DateTime start, DateTime end)
    {
        return await _context.Reservations
            .Where(r => 
                r.SpaceId == spaceId &&
                r.Status == ReservationStatus.Active &&
                r.StartTime < end &&
                r.EndTime > start)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetUserReservations(Guid userId, DateTime start, DateTime end)
    {
        return await _context.Reservations
            .Where(r => 
                r.UserId == userId &&
                r.Status == ReservationStatus.Active &&
                r.StartTime < end &&
                r.EndTime > start)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetFilteredReservations(
        Guid? spaceId, 
        Guid? userId, 
        DateTime? startDate, 
        DateTime? endDate)
    {
        var query = _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Space)
            .AsQueryable();

        if (spaceId.HasValue)
            query = query.Where(r => r.SpaceId == spaceId);
        
        if (userId.HasValue)
            query = query.Where(r => r.UserId == userId);
        
        if (startDate.HasValue)
            query = query.Where(r => r.StartTime >= startDate);
        
        if (endDate.HasValue)
            query = query.Where(r => r.EndTime <= endDate);

        return await query.ToListAsync();
    }
}