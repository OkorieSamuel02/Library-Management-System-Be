using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Request;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Borrowing.Interface
{
    public interface IBorrowService
    {
        Task<Result<string>> IssueBookToMember(CreateLoanRequestModel createLoanRequest);
        Task<Result<string>> ProcessBookReturn(string isbn);
        Task<Result<IList<GetLoanResponseModel>>> GetLoanAsync(GetAllLoanQuery query, string userId);
    }
}
