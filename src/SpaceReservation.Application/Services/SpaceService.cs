using SpaceReservation.Application.DTOs;
using SpaceReservation.Domain.Entities;
using SpaceReservation.Domain.Exceptions;
using SpaceReservation.Domain.Repositories;

namespace SpaceReservation.Application.Services;

public class SpaceService : ISpaceService
{
    private readonly ISpaceRepository _spaceRepository;

    public SpaceService(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<IEnumerable<SpaceDto>> GetAllSpacesAsync()
    {
        var spaces = await _spaceRepository.GetAllAsync();
        return spaces.Select(ToDto);
    }

    public async Task<SpaceDto?> GetSpaceByIdAsync(int id)
    {
        var space = await _spaceRepository.GetByIdAsync(id);
        return space != null ? ToDto(space) : null;
    }

    public async Task<SpaceDto> CreateSpaceAsync(string name, string description)
    {
        var space = new Space(name, description);
        await _spaceRepository.AddAsync(space);
        return ToDto(space);
    }

    public async Task UpdateSpaceAsync(int id, string name, string description)
    {
        var space = await _spaceRepository.GetByIdAsync(id);
        if (space == null)
            throw new DomainException($"Space with ID {id} not found");

        space.Update(name, description);
        await _spaceRepository.UpdateAsync(space);
    }

    public async Task DeleteSpaceAsync(int id)
    {
        var space = await _spaceRepository.GetByIdAsync(id);
        if (space == null)
            throw new DomainException($"Space with ID {id} not found");

        await _spaceRepository.DeleteAsync(space);
    }

    private static SpaceDto ToDto(Space space)
    {
        return new SpaceDto(space.Id, space.Name, space.Description);
    }
} 