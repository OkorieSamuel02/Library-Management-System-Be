using AutoMapper;
using LibraryManagementSystem.Application.Authentication.Interface;
using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Request;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;
using LibraryManagementSystem.Application.Membership.Interface;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementSystem.Infrastructure.Repository.MemberShip
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MemberService> _logger;
        private readonly UserManager<User> _userService;
        private readonly AuthHelper _authHelper;
        private readonly IMapper _mapper;
        public MemberService(ApplicationDbContext context, ILogger<MemberService> logger, UserManager<User> userService, AuthHelper authHelper, IMapper mapper)
        {
            _context = context; 
            _logger = logger;
            _userService = userService;
            _authHelper = authHelper;
            _mapper = mapper;
        }
        public async Task<Result<string>> CreateMember(CreateMemberRequestModel model)
        {
            using var trasaction = await _context.Database.BeginTransactionAsync();
            try
            {
                 var existingMember = await _context.Members.Where(c => c.email == model.email && c.Status == Domain.Enums.MemberStatus.Active).FirstOrDefaultAsync();
                if(existingMember != null)
                {
                    return Result<string>.Failure($"User with email {model.email} already exist", System.Net.HttpStatusCode.Conflict);
                }

                var member = new Member
                {
                    email = model.email,
                    contactNumber = model.contactNumber!,
                    membershipDate = DateTime.UtcNow,
                    name = model.name,
                    phoneNumber = model.phoneNumber!,
                };

                await _context.Members.AddAsync(member);
                var registerNewlyCreatedMember = new User
                {
                    Email = model.email,
                    UserName = model.email,
                    PhoneNumber = model.phoneNumber,
                    Roles = Domain.Enums.Roles.Member
                };

               
                var result = await _userService.CreateAsync(registerNewlyCreatedMember, model.password);
                if(!result.Succeeded)
                {
                    await trasaction.RollbackAsync();
                    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                    return Result<string>.Failure(errors, System.Net.HttpStatusCode.InternalServerError);
                }

               
                await trasaction.CommitAsync();

                return Result<string>.Success("Member created successfully", $"Account Created for new member {member.name}", System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                await trasaction.RollbackAsync();
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> DeactivateMember(string email)
        {
            try
            {
                var activation = false;
                var emailExist = await _context.Members.FirstOrDefaultAsync(c => c.email == email);
                if (emailExist == null)
                {
                    return Result<string>.Failure($"User with email {email} not found", System.Net.HttpStatusCode.NotFound);    
                }

                emailExist.MemberStatus(activation);

                var saved = await _context.SaveChangesAsync();
                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save member deactivation");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save member deactivation", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Member deactivated Successfully", emailExist.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<IList<MemberResponseModel>>> GetMemberAsync(string? email, bool? isActive, int? pageNumber, int? pageSize, string? userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
                IQueryable<Member> members = _context.Members.AsQueryable();

               var PageSize = pageSize ?? 10;
                var PageNumber = pageNumber ?? 1;

                if(user.Roles == Domain.Enums.Roles.Admin || user.Roles == Domain.Enums.Roles.Librarian)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        members = members.Where(c => c.email == email);
                    }

                    if (isActive.HasValue)
                    {
                        members = members.Where(c => c.Status == Domain.Enums.MemberStatus.Active);
                    }

                    members = members.Skip((PageNumber - 1) * PageSize).Take(PageSize);

                    var result = await members.ToListAsync();
                    if(result.Count == 0)
                    {
                        return Result<IList<MemberResponseModel>>.Failure("No Member Found", System.Net.HttpStatusCode.NotFound);
                    }

                    var Response = _mapper.Map<IList<MemberResponseModel>>(result);
                    return Result<IList<MemberResponseModel>>.Success("Members retrieved successfuly", Response, System.Net.HttpStatusCode.OK);

                }
                else
                {
                    members = members.Where(c => c.email == user.Email);

                    members = members.Skip((PageNumber - 1) * PageSize).Take(PageSize);

                    var result = await members.ToListAsync();
                    if (result.Count == 0)
                    {
                        return Result<IList<MemberResponseModel>>.Failure("Member not Found", System.Net.HttpStatusCode.NotFound);
                    }

                    var Response = _mapper.Map<IList<MemberResponseModel>>(result);
                    return Result<IList<MemberResponseModel>>.Success("Member  retrieved successfuly", Response, System.Net.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<IList<MemberResponseModel>>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> ReactivateMember(string email)
        {
            try
            {
                var activation = true;
                var emailExist = await _context.Members.FirstOrDefaultAsync(c => c.email == email);
                if (emailExist == null)
                {
                    return Result<string>.Failure($"User with email {email} not found", System.Net.HttpStatusCode.NotFound);
                }

                emailExist.MemberStatus(activation);

                var saved = await _context.SaveChangesAsync();
                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save member reactivation");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save member reactivation", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Member Reactivated Successfully", emailExist.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
