using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve.Query
{
    public class GetReservationQuery : IRequest<Result<IList<GetReservationResponseModel>>>
    {
        public string? userId { get; set; }
        public string? reservationId { get; set;}
        public string? memberEmail { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
    }

    public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, Result<IList<GetReservationResponseModel>>>
    {
        private readonly IReservationService _reservationService;
        public GetReservationQueryHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public async Task<Result<IList<GetReservationResponseModel>>> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            return await _reservationService.GetAllReservation(request);
        }
    }
}
