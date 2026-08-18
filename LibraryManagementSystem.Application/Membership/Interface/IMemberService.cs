using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Request;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Interface
{
    public interface IMemberService
    {
        Task<Result<string>> CreateMember(CreateMemberRequestModel model);
        Task<Result<string>> ReactivateMember(string email);
        Task<Result<string>> DeactivateMember(string email);
        Task<Result<IList<MemberResponseModel>>> GetMemberAsync(string? email, bool? isActive, int? pageNumber, int? pageSize, string? userId);
    }
}
