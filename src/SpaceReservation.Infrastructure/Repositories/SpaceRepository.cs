using Microsoft.EntityFrameworkCore;
using SpaceReservation.Domain.Entities;
using SpaceReservation.Domain.Repositories;
using SpaceReservation.Infrastructure.Persistence;

namespace SpaceReservation.Infrastructure.Repositories;

public class SpaceRepository : ISpaceRepository
{
    private readonly ApplicationDbContext _context;

    public SpaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Space?> GetByIdAsync(int id)
    {
        return await _context.Spaces.FindAsync(id);
    }

    public async Task<IEnumerable<Space>> GetAllAsync()
    {
        return await _context.Spaces.ToListAsync();
    }

    public async Task<Space> AddAsync(Space space)
    {
        _context.Spaces.Add(space);
        await _context.SaveChangesAsync();
        return space;
    }

    public async Task UpdateAsync(Space space)
    {
        _context.Entry(space).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Space space)
    {
        _context.Spaces.Remove(space);
        await _context.SaveChangesAsync();
    }
} 