using LibraryManagementSystem.Application.Authentication.DataTransferObject.Request;
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

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
                {
                    return Result<string>.Failure("Invalid userName or password.", System.Net.HttpStatusCode.Unauthorized);
                }

                var isValidPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if(!isValidPassword.Succeeded)
                {
                    return Result<string>.Failure("Invalid userName or password", System.Net.HttpStatusCode.Unauthorized);
                }

                var jwtToken = _authHelper.GenerateToken(user);
                return Result<string>.Success("Login successful.", jwtToken, System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
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

                var user = new User
                {
                    Email = register.email,
                    UserName = register.email,
                    PhoneNumber = register.PhoneNumber,
                    Roles = Domain.Enums.Roles.Librarian,
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
