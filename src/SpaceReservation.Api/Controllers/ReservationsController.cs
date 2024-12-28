using Microsoft.AspNetCore.Mvc;
using SpaceReservation.Application.DTOs;
using SpaceReservation.Application.Services;
using SpaceReservation.Domain.Exceptions;

namespace SpaceReservation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations(
        [FromQuery] int? spaceId,
        [FromQuery] string? cedula,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        IEnumerable<ReservationDto> reservations;

        if (startDate.HasValue || endDate.HasValue)
        {
            reservations = await _reservationService.GetReservationsByDateRangeAsync(startDate, endDate);
        }
        else if (spaceId.HasValue)
        {
            reservations = await _reservationService.GetReservationsBySpaceIdAsync(spaceId.Value);
        }
        else if (!string.IsNullOrEmpty(cedula))
        {
            reservations = await _reservationService.GetReservationsByCedulaAsync(cedula);
        }
        else
        {
            reservations = await _reservationService.GetAllReservationsAsync();
        }

        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetReservation(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateReservationRequest request)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(
                request.SpaceId,
                request.Cedula,
                request.StartTime,
                request.EndTime);

            return CreatedAtAction(
                nameof(GetReservation),
                new { id = reservation.Id },
                reservation);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        try
        {
            await _reservationService.DeleteReservationAsync(id);
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreateReservationRequest(
    int SpaceId,
    string Cedula,
    DateTime StartTime,
    DateTime EndTime); 