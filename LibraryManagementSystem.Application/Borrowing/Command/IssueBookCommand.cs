using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Request;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Borrowing.Command
{
    public class IssueBookCommand : CreateLoanRequestModel, IRequest<Result<string>>
    {

    }

    public class IssueBookCommandHandler : IRequestHandler<IssueBookCommand, Result<string>>
    {
        private readonly IBorrowService _borrowService;
        public IssueBookCommandHandler(IBorrowService borrowService)
        {
            _borrowService = borrowService;  
        }
        public async Task<Result<string>> Handle(IssueBookCommand request, CancellationToken cancellationToken)
        {
            return await _borrowService.IssueBookToMember(request);
        }
    }
}
