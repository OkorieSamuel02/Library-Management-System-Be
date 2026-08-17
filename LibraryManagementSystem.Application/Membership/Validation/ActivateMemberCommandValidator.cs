using FluentValidation;
using LibraryManagementSystem.Application.Membership.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Validation
{
    public class ActivateMemberCommandValidator : AbstractValidator<ActivateMemberCommand>
    {
        public ActivateMemberCommandValidator()
        {
            RuleFor(c => c.email).NotEmpty().NotNull()
                 .WithMessage("For member Reactivation email is required");
        }
    }
}
