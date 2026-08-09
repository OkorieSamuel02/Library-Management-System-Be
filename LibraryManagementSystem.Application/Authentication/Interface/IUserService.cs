using LibraryManagementSystem.Application.Authentication.DataTransferObject.Request;
using LibraryManagementSystem.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.Interface
{
    public interface IUserService
    {
        Task<Result<string>> CreateUserAsync(RegisterRequestModel register);
        Task<Result<string>> LoginAsync(string email, string password);
    }
}
