using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.Query
{
    public class GetAllLoanQuery : IRequest<Result<IList<GetLoanResponseModel>>>
    {
        public bool? active { get; set; }
        public bool? isDue {  get; set; }
        public string? memberEmail { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public string userId { get; set; }
    }

    public class GetAllLoanQueryHandler : IRequestHandler<GetAllLoanQuery, Result<IList<GetLoanResponseModel>>>
    {
        private readonly IBorrowService _borrowService;
        public GetAllLoanQueryHandler(IBorrowService borrowService)
        {
            _borrowService = borrowService;   
        }
        public async Task<Result<IList<GetLoanResponseModel>>> Handle(GetAllLoanQuery request, CancellationToken cancellationToken)
        {
            return await _borrowService.GetLoanAsync(request, request.userId);
        }
    }
}
