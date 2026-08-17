using LibraryManagementSystem.Application.Authentication.Command;
using LibraryManagementSystem.Application.ConfigSetting.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    
    [Route("api/")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost]
        [Route("settings")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> CreateSettings(CreateSettingCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPut]
        [Route("settings")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> UpdateSettings(UpdateSettingCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.statusCode, result);
        }
    }
}
