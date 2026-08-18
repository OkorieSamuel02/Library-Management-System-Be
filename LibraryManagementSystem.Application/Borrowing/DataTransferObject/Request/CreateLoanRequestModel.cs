using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Borrowing.DataTransferObject.Request
{
    public class CreateLoanRequestModel
    {
        public string isbn {  get; set; }
        public string memberEmail { get; set;}
    }
}
