using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;
using LibraryManagementSystem.Application.Membership.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Query
{
    public class GetMemberQuery : IRequest<Result<IList<MemberResponseModel>>>
    {
        public bool? active { get; set; }
        public string? memberEmail { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public string userId { get; set; }
    }

    public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, Result<IList<MemberResponseModel>>>
    {
        private readonly IMemberService _memberService;
        public GetMemberQueryHandler(IMemberService memberService)
        {
           _memberService = memberService;
        }
        public async Task<Result<IList<MemberResponseModel>>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            return await _memberService.GetMemberAsync(request.memberEmail, request.active, request.pageNumber, request.pageSize, request.userId);
        }
    }
    
}
