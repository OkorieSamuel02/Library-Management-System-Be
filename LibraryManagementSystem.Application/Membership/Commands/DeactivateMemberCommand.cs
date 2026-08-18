using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Commands
{
    public class DeactivateMemberCommand : IRequest<Result<string>>
    {
        public string email { get; set; } = string.Empty;
    }

    public class DeactivateMemberCommandHandler : IRequestHandler<DeactivateMemberCommand, Result<string>>
    {
        private readonly IMemberService _memberService;
        public DeactivateMemberCommandHandler(IMemberService memberService)
        {
             _memberService = memberService;
        }
        public async Task<Result<string>> Handle(DeactivateMemberCommand request, CancellationToken cancellationToken)
        {
            return await _memberService.DeactivateMember(request.email);
        }
    }
}
