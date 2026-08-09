using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.DataTransferObject.Request
{
    public class RegisterRequestModel
    {
        public string email {  get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
