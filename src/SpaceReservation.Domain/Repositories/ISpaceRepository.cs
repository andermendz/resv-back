using SpaceReservation.Domain.Entities;

namespace SpaceReservation.Domain.Repositories;

public interface ISpaceRepository
{
    Task<Space?> GetByIdAsync(int id);
    Task<IEnumerable<Space>> GetAllAsync();
    Task<Space> AddAsync(Space space);
    Task UpdateAsync(Space space);
    Task DeleteAsync(Space space);
} 