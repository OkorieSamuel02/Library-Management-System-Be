using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;
using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response
{
    public class GetLoanResponseModel
    {
        public Guid id { get; set; }
        public DateTime issueDate { get; set; } = DateTime.UtcNow;
        public DateTime dueDate { get; private set; }
        public DateTime? returnDate { get; set; }
        public decimal fineAmount { get; private set; } = 0;
        public BookResponseModel? Book { get; set; }
        public MemberResponseModel? Member { get; set; }
        public string status { get; set; }
        public bool IsFinePaid { get; set; }
    }
}
