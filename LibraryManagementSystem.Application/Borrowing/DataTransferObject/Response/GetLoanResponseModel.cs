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
        public DateTime issueDate { get; set; } 
        public DateTime dueDate { get;  set; }
        public DateTime? returnDate { get; set; }
        public decimal fineAmount { get; set; } 
        public BookResponseModel? Book { get; set; }
        public MemberResponseModel? Member { get; set; }
        public string? status { get; set; }
        public bool isFinePaid { get; set; }
    }

    public class GetPersonalLoanResponse
    {
        public Guid id { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime dueDate { get;  set; }
        public DateTime? returnDate { get; set; }
        public decimal fineAmount { get;  set; } 
        public BookResponseModel? Book { get; set; }
        public string? status { get; set; }
        public bool isFinePaid { get; set; }
    }
}
