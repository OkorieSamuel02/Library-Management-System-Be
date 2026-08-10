using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Book.Interface;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LibraryManagementSystem.Application.Book.DataTransferObject.Request;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagementSystem.Infrastructure.Repository.BookCatalog
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookService> _logger;
        public BookService(ApplicationDbContext context, ILogger<BookService> logger)
        {
            _context = context;  
            _logger = logger;
        }
        public async Task<Result<string>> CreateBookAsync(CreateBookRequestModel create)
        {
            try
            {
                var existingIsbn = await _context.Books.FirstOrDefaultAsync(c => EF.Functions.ILike(c.isbn, create.isbn));
                if(existingIsbn != null)
                {
                     return Result<string>.Failure($"Book with isbn:{create.isbn} already exist", System.Net.HttpStatusCode.Conflict);
                }

                var book = new Book
                {
                    id = Guid.NewGuid(),
                    author = create.author,
                    isbn = create.isbn,
                    title = create.title,
                    genre = create.genre,
                    totalCopies = create.numberOfCopies,
                    createAt = DateTime.UtcNow,
                    updatedAt = DateTime.UtcNow,
                };

                 await _context.Books.AddAsync(book);
                var saved = await _context.SaveChangesAsync();
                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save books");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save books", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Book Created Successfully", book.id.ToString(), System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> RemoveBookAsync(string isbn)
        {
            try
            {
                var checkingIfBookExist = await _context.Books.FirstOrDefaultAsync(c => EF.Functions.ILike(c.isbn, isbn));
                if(checkingIfBookExist == null)
                {
                    return Result<string>.Failure($"No Book with Provided isbn:{isbn} found", System.Net.HttpStatusCode.BadRequest);
                }

                 _context.Books.Remove(checkingIfBookExist);
                var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save books");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save books", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Book Removed Successfully", checkingIfBookExist.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> UpdateBookAsync(UpdateBookRequestModel update)
        {
            try
            {
                var checkIfBookExist = await _context.Books.FirstOrDefaultAsync(c => EF.Functions.ILike(c.isbn, update.isbn));
                if (checkIfBookExist == null)
                {
                    return Result<string>.Failure($"No Book with Provided isbn:{update.isbn} found", System.Net.HttpStatusCode.BadRequest);
                }


                checkIfBookExist.BooksUpdate(update.numberOfCopies);
                checkIfBookExist.author = update.author ?? checkIfBookExist.author;
                checkIfBookExist.isbn = update.isbn ?? checkIfBookExist.isbn;
                checkIfBookExist.title = update.title ?? checkIfBookExist.title;
                checkIfBookExist.genre = update.genre ?? checkIfBookExist.genre;
               

                 _context.Books.Update(checkIfBookExist);
                var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save books");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save books", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Book Updated Successfully", checkIfBookExist.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public Task<Result<IEnumerable<BookResponseModel>>> ViewBooksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
