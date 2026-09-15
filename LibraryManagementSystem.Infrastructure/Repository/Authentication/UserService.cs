using LibraryManagementSystem.Application.Authentication.DataTransferObject.Request;
using LibraryManagementSystem.Application.Authentication.DataTransferObject.Response;
using LibraryManagementSystem.Application.Authentication.Interface;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repository.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserService> _logger;
        private readonly AuthHelper _authHelper;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserService> logger, AuthHelper authHelper)
        {
             _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _authHelper = authHelper;
        }

        public async Task<Result<LoginResponseModel>> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
                {
                    return Result<LoginResponseModel>.Failure("Invalid userName or password.", System.Net.HttpStatusCode.Unauthorized);
                }

                var isValidPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if(!isValidPassword.Succeeded)
                {
                    return Result<LoginResponseModel>.Failure("Invalid userName or password", System.Net.HttpStatusCode.Unauthorized);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var primaryRole = roles.FirstOrDefault() ?? "User";

                var jwtToken = _authHelper.GenerateToken(user);

                var responseData = new LoginResponseModel
                {
                    Data = jwtToken,
                    User = new UserDto
                    {
                        email = user.Email ?? string.Empty,
                        role = primaryRole,
                        firstName = user.firstName,
                        lastName = user.lastName,
                    }
                };
                return Result<LoginResponseModel>.Success("Login successful.", responseData, System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<LoginResponseModel>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> CreateUserAsync(RegisterRequestModel register)
        {
            try
            {
                var checkIfUserExist = await _userManager.FindByEmailAsync(register.email);
                if(checkIfUserExist != null)
                {
                    return Result<string>.Failure("User with provided email already exist", System.Net.HttpStatusCode.Conflict);
                }

                var role = Domain.Enums.Roles.Admin;

                switch (register.role)
                {
                    case 1:
                        role = Domain.Enums.Roles.Admin;
                        break;
                    case 2:
                        role = Domain.Enums.Roles.Librarian;
                        break;
                    case 3:
                        role = Domain.Enums.Roles.Member;
                        break;
                    default:
                        role = Domain.Enums.Roles.Librarian;
                        break;

                }

                var user = new User
                {
                    Email = register.email,
                    UserName = register.email,
                    PhoneNumber = register.contactNumber,
                    contactNumber = register.contactNumber,
                    Roles = role
                };

                var createUserAsync = await _userManager.CreateAsync(user, register.password);
                if (!createUserAsync.Succeeded)
                {
                    var errors = string.Join(", ",createUserAsync.Errors.Select(x => x.Description));
                    return Result<string>.Failure(errors, System.Net.HttpStatusCode.BadRequest);
                }

                return Result<string>.Success("User created successfully", user.Id, System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
