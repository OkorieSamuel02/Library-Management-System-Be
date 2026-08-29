using AutoMapper;
using AutoMapper.Execution;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Request;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.Interface;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Reserve;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Request;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Response;
using LibraryManagementSystem.Application.Reserve.Query;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Helper;
using LibraryManagementSystem.Infrastructure.Repository.MemberShip;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repository.Reserves
{
    public class ReservationService : IReservationService
    {
       
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservationService> _logger;
        private readonly IMapper _mapper;
        private readonly IBorrowService _borrowService;
       
        public ReservationService(ApplicationDbContext context, ILogger<ReservationService> logger, IMapper mapper, IBorrowService borrowService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _borrowService = borrowService; 
        }

        public async Task<Result<string>> AddedReservation(CreateReservationRequest request)
        {
            try
            {
                var findBookAsync = await _context.Books.FirstOrDefaultAsync(c => c.isbn == request.Isbn);
                if (findBookAsync == null)
                {
                    return Result<string>.Failure($"No Book with Provided isbn:{request.Isbn} found", System.Net.HttpStatusCode.BadRequest);
                }

                if(findBookAsync.availableCopies > 0)
                {
                    return Result<string>.Failure("Book is currently available. You do not need to make a reservation.",  System.Net.HttpStatusCode.BadRequest);
                }

                var findMember = await _context.Members.FirstOrDefaultAsync(c => c.email == request.memberEmail);

                if (findMember == null)
                {
                    return Result<string>.Failure("Member not found", System.Net.HttpStatusCode.NotFound);
                }

                var reservations = new Reservation
                {
                    id = Guid.NewGuid(),
                    books = findBookAsync,
                    members = findMember,
                };

                await _context.Reservations.AddAsync(reservations);
               var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to add reservation");
                    return Result<string>.Failure($"An unexpected error occurred while trying to add reservation", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($" reservation added Successfully", reservations.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> CancelReservation(Guid reservationId)
        {
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(c => c.id == reservationId);
                if (reservation == null)
                {
                    return Result<string>.Failure($"Reservation with {reservationId} not found", System.Net.HttpStatusCode.NotFound);
                }

                reservation.CancelReservation();
                var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to cancel reservation");
                    return Result<string>.Failure($"An unexpected error occurred while trying to cancel reservation", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($" reservation cancelled Successfully", reservation.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> ClaimReservation(Guid reservationId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservation = await _context.Reservations.FirstOrDefaultAsync(c => c.id == reservationId);
                if (reservation == null)
                {
                    return Result<string>.Failure($"Reservation with {reservationId} not found", System.Net.HttpStatusCode.NotFound);
                }

                reservation.UpdateStatus(Domain.Enums.ReservationStatus.Fulfilled);

                var request = new CreateLoanRequestModel
                {
                    isbn = reservation.books.isbn,
                    memberEmail = reservation.members.email
                };


                await _borrowService.IssueBookToMember(request);

                var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to claim reservation");
                    return Result<string>.Failure($"An unexpected error occurred while trying to claim reservation", System.Net.HttpStatusCode.InternalServerError);
                }

                await transaction.CommitAsync();    

                return Result<string>.Success($" reservation claimed Successfully", reservation.id.ToString(), System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

       
        public async Task<Result<IList<GetReservationResponseModel>>> GetAllReservation(GetReservationQuery query)
        {
            try
            {
                var pageNumber = query.pageNumber ?? 1;
                var pageSize = query.pageSize ?? 10;
                 IQueryable<Reservation> reservations =  _context.Reservations.Include(c => c.books).Include(c => c.members).AsQueryable();
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == query.userId);
                if(user.Roles == Domain.Enums.Roles.Admin || user.Roles == Domain.Enums.Roles.Librarian)
                {
                    if (!string.IsNullOrEmpty(query.memberEmail))
                    {
                        reservations = reservations.Where(c => c.members.email ==  query.memberEmail);
                    }

                    if(!string.IsNullOrEmpty(query.reservationId))
                    {
                        reservations = reservations.Where(c => c.id.ToString() == query.reservationId);
                    }

                    reservations = reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                    var result = await reservations.ToListAsync();
                    if (result.Count <= 0)
                    {
                        return Result<IList<GetReservationResponseModel>>.Failure("No loan Found", System.Net.HttpStatusCode.InternalServerError);
                    }

                    var response = _mapper.Map<IList<GetReservationResponseModel>>(result);
                    return Result<IList<GetReservationResponseModel>>.Success("Reservations retrieved successfuly", response, System.Net.HttpStatusCode.OK);
                }
                else
                {
                    reservations = reservations.Where(c => c.members!.email == user.Email);

                    reservations = reservations.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                    var result = await reservations.ToListAsync();

                    var personalLoans = _mapper.Map<IList<GetPersonalReservationResponse>>(result);

                    var response = personalLoans.Select(x => new GetReservationResponseModel
                    {
                        id = x.id,
                        holdExpiresAt = x.holdExpiresAt,
                        reservationDate = x.reservationDate,
                        reservationStatus = x.reservationStatus,
                        books = x.books,

                    }).ToList();

                    return Result<IList<GetReservationResponseModel>>.Success("Reservations retrieved successfully", response, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<IList<GetReservationResponseModel>>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
