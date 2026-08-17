using FluentValidation;
using LibraryManagementSystem.Application.Authentication.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.Validation
{
    public class RegisterCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.email).NotEmpty().NotNull()
                   .WithMessage("email cannot be null or empty").EmailAddress().WithMessage("please enter a valid email");

            RuleFor(c => c.role).GreaterThan(0).LessThan(4).WithMessage("Role is between 1-Admin, 2-Librarian, 3-Member");
        }
    }
}
