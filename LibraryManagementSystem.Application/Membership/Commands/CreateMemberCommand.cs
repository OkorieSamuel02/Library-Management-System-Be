using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Request;
using LibraryManagementSystem.Application.Membership.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Commands
{
    public class CreateMemberCommand : CreateMemberRequestModel, IRequest<Result<string>>
    {

    }

    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<string>>
    {
        private readonly IMemberService _memberService;
        public CreateMemberCommandHandler(IMemberService memberService)
        {
            _memberService = memberService;  
        }
        public Task<Result<string>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
           return _memberService.CreateMember(request);
        }
    }
}
