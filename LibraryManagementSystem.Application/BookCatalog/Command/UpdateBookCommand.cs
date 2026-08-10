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
    public class UpdateBookCommand : UpdateBookRequestModel, IRequest<Result<string>>
    {

    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<string>>
    {
        private readonly IBookService _bookService;
        public UpdateBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<Result<string>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.UpdateBookAsync(request);
        }
    }
}
