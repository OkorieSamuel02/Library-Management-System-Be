using LibraryManagementSystem.Application.Authentication.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
             _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPost]
        [Route("users")]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }
    }
}
