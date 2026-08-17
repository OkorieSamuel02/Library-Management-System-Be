using FluentValidation;
using LibraryManagementSystem.Application.BookCatalog.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.BookCatalog.Validation
{
    public class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
    {
        public RemoveBookCommandValidator()
        {
            RuleFor(c => c.isbn).NotNull().NotEmpty()
               .WithMessage("For book removal isbn is required");
        }
    }
}
