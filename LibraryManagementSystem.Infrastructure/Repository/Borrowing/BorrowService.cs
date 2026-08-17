using AutoMapper;
using Azure;
using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Request;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Membership.Query;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repository.Borrowing
{
    public class BorrowService : IBorrowService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BorrowService> _logger;
        private readonly IMapper _mapper;
        public BorrowService(ApplicationDbContext context, ILogger<BorrowService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<IList<GetLoanResponseModel>>> GetLoanAsync(GetAllLoanQuery query, string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
                IQueryable<Loan> loans = _context.Loans.Include(c => c.Member).AsQueryable();

                var pageNumber = query.pageNumber ?? 1;
                var pageSize = query.pageSize ?? 10;

                if(user.Roles == Domain.Enums.Roles.Admin || user.Roles == Domain.Enums.Roles.Librarian)
                {
                    if (query.active.HasValue)
                    {
                        loans = loans.Where(c => c.status == Domain.Enums.LoanStatus.Active);
                    }

                    if (query.isDue.HasValue)
                    {
                        loans = loans.Where(c => c.dueDate > DateTime.UtcNow);
                    }

                    if (!string.IsNullOrEmpty(query.memberEmail))
                    {
                        loans = loans.Where(c => c.Member.email == query.memberEmail);
                    }

                    loans = loans.OrderByDescending(c => c.dueDate);

                    loans = loans.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                    var result = await loans.ToListAsync();
                    if (result.Count <= 0)
                    {
                        return Result<IList<GetLoanResponseModel>>.Failure("No loan Found", System.Net.HttpStatusCode.InternalServerError);
                    }

                    var response = _mapper.Map<IList<GetLoanResponseModel>>(result);
                    return Result<IList<GetLoanResponseModel>>.Success("Loans retrieved successfuly", response, System.Net.HttpStatusCode.OK);
                }
                else
                {
                    loans = loans.Where(c => c.Member!.email == user.Email);

                    //loans = loans.OrderBy(c => c.status);

                    loans = loans.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                    var result = await loans.ToListAsync();
                    if (result.Count <= 0)
                    {
                        return Result<IList<GetLoanResponseModel>>.Failure("No loan Found", System.Net.HttpStatusCode.InternalServerError);
                    }

                    var response = _mapper.Map<IList<GetLoanResponseModel>>(result);
                    return Result<IList<GetLoanResponseModel>>.Success("Loans retrieved successfuly", response, System.Net.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<IList<GetLoanResponseModel>>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> IssueBookToMember(CreateLoanRequestModel createLoanRequest)
        {
            try
            {
                var checkBookExist = await _context.Books.Where(c => c.isbn == createLoanRequest.isbn).FirstOrDefaultAsync();
                if (checkBookExist == null)
                {
                    return Result<string>.Failure($"No Book with Provided isbn:{createLoanRequest.isbn} was found", System.Net.HttpStatusCode.BadRequest);
                }

                if (checkBookExist.availableCopies <= 0)
                {
                    return Result<string>.Failure($"Book with Provided isbn:{createLoanRequest.isbn} is currently not available", System.Net.HttpStatusCode.BadRequest);
                }

                var memberEligibility = await _context.Members.Where(c => c.email == createLoanRequest.memberEmail).FirstOrDefaultAsync();
                if (memberEligibility == null)
                {
                    return Result<string>.Failure($"No member with Provided email:{createLoanRequest.memberEmail} was found", System.Net.HttpStatusCode.BadRequest);
                }

                var activeLoanCount = memberEligibility!.loans!.Where(c => c.status == Domain.Enums.LoanStatus.Active).Count();

                if (memberEligibility.Status == Domain.Enums.MemberStatus.Suspended)
                {
                    return Result<string>.Failure($"you are not enligible for the loan because your account is suspended", System.Net.HttpStatusCode.BadRequest);
                }

                var setting = await _context.Settings.FirstOrDefaultAsync();

                if (setting == null)
                {
                    return Result<string>.Failure("System settings have not been configured.", HttpStatusCode.InternalServerError);
                }

                if (activeLoanCount >= setting.maxActiveLoans)
                {
                    return Result<string>.Failure($"You have reached the maximum number of active loans.", System.Net.HttpStatusCode.BadRequest);
                }

                var outstandingFine = await _context.Loans.Where(l => l.memberId == memberEligibility.id && l.IsFinePaid == false).SumAsync(l => l.fineAmount);

                if (outstandingFine > setting.UnpaidFinethreshold)
                {
                    return Result<string>.Failure($"You outstanding balance of {outstandingFine}. is above the Unpaid Fine threshold", System.Net.HttpStatusCode.BadRequest);
                }

                var loan = new Loan
                {
                    issueDate = DateTime.UtcNow,
                    Book = checkBookExist,
                    Member = memberEligibility,
                    returnDate = null,
                };

                loan.CalculateDueDate(setting.loanPeriodDays);
                checkBookExist.BorrowBook();


                var addbook = await _context.Loans.AddAsync(loan);
                await _context.SaveChangesAsync();

                return Result<string>.Success($"Book titled:: {checkBookExist.title} borrowed to {memberEligibility.name}", loan.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> ProcessBookReturn(string isbn)
        {
            try
            {
                var currentDate = DateTime.UtcNow;
              //  var bookexist = await _
                var loans = await _context.Loans.Where(c => c.Book.isbn == isbn && c.status == Domain.Enums.LoanStatus.Active).Include(c => c.Book).FirstOrDefaultAsync();
                if(loans == null)
                {
                    return Result<string>.Failure($"loan already processed", System.Net.HttpStatusCode.BadRequest);
                }

                var setting = await _context.Settings.FirstOrDefaultAsync();
                if(setting == null)
                {
                    return Result<string>.Failure("System settings have not been configured.", HttpStatusCode.InternalServerError);
                }

                loans.Return(currentDate, setting.fineRatePerDay);

                loans.Book.ReturnBook();

                await _context.SaveChangesAsync();

                return Result<string>.Success($"loan processed", loans.id.ToString(), System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
