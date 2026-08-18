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
    public class ActivateMemberCommand : IRequest<Result<string>>
    {
        public string email { get; set; } = string.Empty;
    }

    public class ActivateMemberCommandHandler : IRequestHandler<ActivateMemberCommand, Result<string>>
    {
        private readonly IMemberService _memberService;
        public ActivateMemberCommandHandler(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<Result<string>> Handle(ActivateMemberCommand request, CancellationToken cancellationToken)
        {
            return await _memberService.ReactivateMember(request.email);
        }
    }
}
