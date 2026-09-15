using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.DataTransferObject.Response
{
    public class LoginResponseModel
    {
        public string Data { get; set; } = string.Empty; 
        public UserDto User { get; set; } = new UserDto();
    }

    public class UserDto
    {
        public string email { get; set; } = string.Empty;
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string role { get; set; } = string.Empty;
    }
}
