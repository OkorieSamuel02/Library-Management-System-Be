using LibraryManagementSystem.Application.BookCatalog.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatalogController(IMediator mediator)
        {
              _mediator = mediator;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBookAsync(CreateBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateBookAsync(UpdateBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }


        [HttpPut]
        [Route("remove")]
        public async Task<IActionResult> CreateBookAsync(RemoveBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }
    }
}
