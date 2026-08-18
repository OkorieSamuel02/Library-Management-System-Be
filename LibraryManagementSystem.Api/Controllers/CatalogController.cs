using LibraryManagementSystem.Application.BookCatalog.Command;
using LibraryManagementSystem.Application.BookCatalog.Query;
using LibraryManagementSystem.Application.Membership.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Api.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> CreateBookAsync(CreateBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> UpdateBookAsync(UpdateBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }


        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> CreateBookAsync(RemoveBookCommand createBook)
        {
            var result = await _mediator.Send(createBook);
            return StatusCode((int)result.statusCode, result);
        }

        [HttpGet]
        [Route("books")]
       // [Authorize(Roles = "Admin,Librarian,Member")]
        public async Task<IActionResult> GetAllBooks(string? title, string? author, string? isbn, string? genre, int? pageNumber, int? pageSize)
        {
            var query = new GetBooksQuery()
            {
                author = author,
                genre = genre,
                isbn = isbn,
                title = title,
                pageNumber = pageNumber,
                pageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return StatusCode((int)result.statusCode, result);

           
        }
    }
}
