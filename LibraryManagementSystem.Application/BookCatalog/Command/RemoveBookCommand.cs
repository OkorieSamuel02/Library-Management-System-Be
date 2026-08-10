using LibraryManagementSystem.Application.Book.Interface;
using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.BookCatalog.Command
{
    public class RemoveBookCommand : IRequest<Result<string>>
    {
        public string isbn {  get; set; } = string.Empty;
    }

    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, Result<string>>
    {
        private readonly IBookService _bookService;
        public RemoveBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService; 
        }
        public async Task<Result<string>> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.RemoveBookAsync(request.isbn);
        }
    }
}
