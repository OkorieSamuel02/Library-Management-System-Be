using LibraryManagementSystem.Application.Authentication.DataTransferObject.Response;
using LibraryManagementSystem.Application.Authentication.Interface;
using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Authentication.Command
{
    public class LoginCommand : IRequest<Result<LoginResponseModel>>
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseModel>>
    {
        private readonly IUserService _userService;
        public LoginCommandHandler(IUserService userService)
        {
             _userService = userService;
        }
        public Task<Result<LoginResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _userService.LoginAsync(request.email, request.password);
        }
    }
}
