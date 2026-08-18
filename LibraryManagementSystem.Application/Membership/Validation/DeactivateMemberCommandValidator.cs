using FluentValidation;
using LibraryManagementSystem.Application.Membership.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Validation
{
    public class DeactivateMemberCommandValidator : AbstractValidator<DeactivateMemberCommand>
    {
        public DeactivateMemberCommandValidator()
        {
            RuleFor(c => c.email).NotEmpty().NotNull()
             .WithMessage("For member Deactivation email is required");
        }
    }
}
