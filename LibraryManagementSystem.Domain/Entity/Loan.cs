using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Entity
{
    public class Loan
    {
        public Guid id { get; set; }
        public DateTime issueDate { get; set; } = DateTime.UtcNow;
        public DateTime dueDate { get; private set; }
        public DateTime? returnDate { get; set; }
        public decimal fineAmount { get; private set; } = 0;
        public Guid bookId {  get; set; }
        public Book? Book { get;  set; }
        public Guid memberId {  get; set; }
        public Member? Member { get;  set; }
        public LoanStatus status { get; set; } = LoanStatus.Active;
        public bool IsFinePaid { get; set; }

        public void CalculateDueDate(int loanPeriod)
        {
            var loanduedate = issueDate.AddDays(loanPeriod);
            dueDate = loanduedate.Date;
        }

      
        public void Return(DateTime currentDate, decimal fineRate)
        {
            returnDate = currentDate;

            if (currentDate.Date > dueDate.Date)
            {
                var overdueDays = (currentDate.Date - dueDate.Date).Days;
                fineAmount = overdueDays * fineRate;
            }
            else
            {
                fineAmount = 0;
            }

            status = LoanStatus.Returned;
        }

    }
}
