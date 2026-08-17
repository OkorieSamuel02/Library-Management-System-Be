using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Book.Interface;
using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.BookCatalog.Query
{
    public class GetBooksQuery : IRequest<Result<IList<BookResponseModel>>>
    {
        public string? title { get; set; } 
        public string? author { get; set; } 
        public string? isbn { get; set; } 
        public string? genre { get; set; } 
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
    }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<IList<BookResponseModel>>>
    {
        private readonly IBookService _bookService;
        public GetBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService; 
        }
        public async Task<Result<IList<BookResponseModel>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.ViewBooksAsync(request);
        }
    }
}
