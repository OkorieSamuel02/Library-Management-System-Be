using FluentValidation;
using LibraryManagementSystem.Application.Authentication.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.Validation
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.email).NotEmpty().NotNull()
                  .WithMessage("email cannot be null or empty")
                   .EmailAddress().WithMessage("please enter a valid email");

            RuleFor(c => c.password).NotEmpty().NotNull()
                   .WithMessage("Password cannot be null or empty");
        }
    }
}
