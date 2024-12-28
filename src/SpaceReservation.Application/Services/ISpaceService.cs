using SpaceReservation.Application.DTOs;

namespace SpaceReservation.Application.Services;

public interface ISpaceService
{
    Task<IEnumerable<SpaceDto>> GetAllSpacesAsync();
    Task<SpaceDto?> GetSpaceByIdAsync(int id);
    Task<SpaceDto> CreateSpaceAsync(string name, string description);
    Task UpdateSpaceAsync(int id, string name, string description);
    Task DeleteSpaceAsync(int id);
} 