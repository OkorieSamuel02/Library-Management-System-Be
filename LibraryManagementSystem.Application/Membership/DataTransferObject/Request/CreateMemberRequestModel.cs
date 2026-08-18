using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Membership.DataTransferObject.Request
{
    public class CreateMemberRequestModel
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? contactNumber { get; set; } 
        public string? phoneNumber { get; set; }
        public string password { get; set; } = string.Empty;

    }
}
