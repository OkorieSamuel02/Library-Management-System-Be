using LibraryManagementSystem.Application.Authentication.Command;
using LibraryManagementSystem.Application.BookCatalog.Query;
using LibraryManagementSystem.Application.Borrowing.Command;
using LibraryManagementSystem.Application.Membership.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BorrowingReturningController : ControllerBase
    {
 
        private readonly IMediator _mediator;
        public BorrowingReturningController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("issueBook")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> IssueBook(IssueBookCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPost]
        [Route("processReturn")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> ProcessReturn(ProcessReturnCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpGet]
        [Route("loan")]
        [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> GetAllLoans(bool? active, bool? isDue, string? memberEmail, int? pageNumber, int? pageSize)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var query = new GetAllLoanQuery()
            {
                active = active,
                isDue = isDue,
                memberEmail = memberEmail,
                pageNumber = pageNumber,
                pageSize = pageSize,
                userId = user!
            };
            var result = await _mediator.Send(query);
            return StatusCode((int)result.statusCode, result);
        }


    }
}
