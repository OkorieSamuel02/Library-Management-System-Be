using LibraryManagementSystem.Application.BookCatalog.Command;
using LibraryManagementSystem.Application.Membership.Commands;
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
    public class AdminController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("createMember")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> createMember(CreateMemberCommand createMember)
        {
            var result = await _mediator.Send(createMember);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPut]
        [Route("reactivatemember")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> ReactivateMember(ActivateMemberCommand activateMember)
        {
            var result = await _mediator.Send(activateMember);
            return StatusCode((int)result.statusCode, result);
        }


        [HttpPut]
        [Route("deactivatemember")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> DeactivateMember(DeactivateMemberCommand deactivateMember)
        {
            var result = await _mediator.Send(deactivateMember);
            return StatusCode((int)result.statusCode, result);
        }


        [HttpGet]
        [Route("member")]
        [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> GetAllBooks(bool? active, string? memberEmail, int? pageNumber, int? pageSize)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetMemberQuery()
            {
                active = active,
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
