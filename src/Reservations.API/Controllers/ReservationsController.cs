using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reservations.Application.Commands;
using Reservations.Application.DTOs;
using Reservations.Application.Queries;

namespace Reservations.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] Guid? userId,
            [FromQuery] Guid? spaceId)
        {
            var query = new GetReservationsQuery(startDate, endDate, userId, spaceId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateReservation(CreateReservationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelReservation(Guid id)
        {
            await _mediator.Send(new CancelReservationCommand(id));
            return NoContent();
        }
    }
}