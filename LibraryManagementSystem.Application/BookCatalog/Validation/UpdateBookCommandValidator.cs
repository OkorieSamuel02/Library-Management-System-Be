using FluentValidation;
using LibraryManagementSystem.Application.BookCatalog.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.BookCatalog.Validation
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(c => c.isbn).NotEmpty().NotNull()
                 .WithMessage("For book update isbn is required");
        }
    }
}
