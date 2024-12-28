using Microsoft.AspNetCore.Mvc;
using SpaceReservation.Application.DTOs;
using SpaceReservation.Application.Services;
using SpaceReservation.Domain.Exceptions;

namespace SpaceReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpacesController : ControllerBase
{
    private readonly ISpaceService _spaceService;

    public SpacesController(ISpaceService spaceService)
    {
        _spaceService = spaceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpaceDto>>> GetSpaces()
    {
        var spaces = await _spaceService.GetAllSpacesAsync();
        return Ok(spaces);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpaceDto>> GetSpace(int id)
    {
        var space = await _spaceService.GetSpaceByIdAsync(id);
        if (space == null)
            return NotFound();

        return Ok(space);
    }

    [HttpPost]
    public async Task<ActionResult<SpaceDto>> CreateSpace([FromBody] CreateSpaceRequest request)
    {
        try
        {
            var space = await _spaceService.CreateSpaceAsync(request.Name, request.Description);
            return CreatedAtAction(nameof(GetSpace), new { id = space.Id }, space);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSpace(int id, [FromBody] UpdateSpaceRequest request)
    {
        try
        {
            await _spaceService.UpdateSpaceAsync(id, request.Name, request.Description);
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpace(int id)
    {
        try
        {
            await _spaceService.DeleteSpaceAsync(id);
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreateSpaceRequest(string Name, string Description);
public record UpdateSpaceRequest(string Name, string Description); 