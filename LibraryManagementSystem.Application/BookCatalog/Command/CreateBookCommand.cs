using LibraryManagementSystem.Application.Book.DataTransferObject.Request;
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
    public class CreateBookCommand : CreateBookRequestModel, IRequest<Result<string>>
    {

    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<string>>
    {
        private readonly IBookService _bookService;
        public CreateBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService; 
        }
        public async Task<Result<string>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.CreateBookAsync(request);
        }
    }
}
