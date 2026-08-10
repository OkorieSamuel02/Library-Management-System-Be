using FluentValidation;
using LibraryManagementSystem.Application.BookCatalog.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.BookCatalog.Validation
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(c => c.title).NotEmpty()
               .NotNull().WithMessage("title cannot be null or emtpy");

            RuleFor(c => c.author).NotEmpty()
               .NotNull().WithMessage("author cannot be null or emtpy");

            RuleFor(c => c.isbn).NotEmpty()
               .NotNull().WithMessage("isbn cannot be null or emtpy");

            RuleFor(c => c.genre).NotEmpty()
               .NotNull().WithMessage("genre cannot be null or emtpy");

            RuleFor(c => c.numberOfCopies).GreaterThan(0)
                           .WithMessage("number Of Copies must be greater than zero");
        }
    }
}
