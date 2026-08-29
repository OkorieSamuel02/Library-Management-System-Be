using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Request;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Response;
using LibraryManagementSystem.Application.Reserve.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve
{
    public interface IReservationService
    {
        Task<Result<string>> AddedReservation(CreateReservationRequest request);
        Task<Result<IList<GetReservationResponseModel>>> GetAllReservation(GetReservationQuery query);
        Task<Result<string>> CancelReservation(Guid reservationId);
        Task<Result<string>> ClaimReservation(Guid reservationId);
    }
}
