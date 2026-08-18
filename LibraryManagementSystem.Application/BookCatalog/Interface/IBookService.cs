using LibraryManagementSystem.Application.Book.DataTransferObject.Request;
using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.BookCatalog.Query;
using LibraryManagementSystem.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Book.Interface
{
    public interface IBookService
    {
        Task<Result<string>> CreateBookAsync(CreateBookRequestModel create);
        Task<Result<string>> UpdateBookAsync(UpdateBookRequestModel update);
        Task<Result<string>> RemoveBookAsync(string isbn);
        Task<Result<IList<BookResponseModel>>> ViewBooksAsync(GetBooksQuery booksQuery);
      
    }
}
