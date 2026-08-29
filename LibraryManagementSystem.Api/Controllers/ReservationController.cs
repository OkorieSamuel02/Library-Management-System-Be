using LibraryManagementSystem.Application.BookCatalog.Command;
using LibraryManagementSystem.Application.BookCatalog.Query;
using LibraryManagementSystem.Application.Reserve.Command;
using LibraryManagementSystem.Application.Reserve.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("bookReservation")]
        [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> bookReservationAsync(AddReservationCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPut]
        [Route("claimReservation")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> claimReservationAsync(ClaimReservationCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }


        [HttpPut]
        [Route("cancelReservation")]
        [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> cancelReservationAsync(DeleteReservationCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpGet]
        [Route("reservations")]
        [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> GetAllReservation(string? reservationId, string? memberEmail, int? pageNumber, int? pageSize)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetReservationQuery()
            {
                memberEmail = memberEmail,
                reservationId = reservationId,
                pageNumber = pageNumber,
                pageSize = pageSize,
                 userId = user
            };
            var result = await _mediator.Send(query);
            return StatusCode((int)result.statusCode, result);


        }
    }
}
