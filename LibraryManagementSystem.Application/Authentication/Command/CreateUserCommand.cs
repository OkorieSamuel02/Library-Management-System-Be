using LibraryManagementSystem.Application.Authentication.DataTransferObject.Request;
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
    public class CreateUserCommand : RegisterRequestModel, IRequest<Result<string>>
    {

    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
    {
        private readonly IUserService _userService;
        public CreateUserCommandHandler(IUserService userService)
        {
             _userService = userService;
        }
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(request);
        }
    }
}
