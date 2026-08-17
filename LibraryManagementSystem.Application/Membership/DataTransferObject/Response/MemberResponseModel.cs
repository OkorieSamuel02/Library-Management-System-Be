using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.DataTransferObject.Response
{
    public class MemberResponseModel
    {
        public Guid id { get; set; }
        public string? name { get; set; } 
        public string? email { get; set; }
        public string? contactNumber { get; set; } 
        public string? phoneNumber { get; set; } 
        public DateTime membershipDate { get; set; }
        public string Status { get; set; }
    }
}
