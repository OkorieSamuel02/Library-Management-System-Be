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
    public class ProcessReturnCommand : IRequest<Result<string>>
    {
        public string isbn {  get; set; } = string.Empty;
    }

    public class ProcessReturnCommandHandler : IRequestHandler<ProcessReturnCommand, Result<string>>
    {
        private readonly IBorrowService _borrowService;
        public ProcessReturnCommandHandler(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }
        public async Task<Result<string>> Handle(ProcessReturnCommand request, CancellationToken cancellationToken)
        {
            return await _borrowService.ProcessBookReturn(request.isbn);
        }
    }
}
