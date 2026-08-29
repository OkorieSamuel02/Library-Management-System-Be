using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.Reserve.DataTransferObject.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve.Command
{
    public class DeleteReservationCommand :  IRequest<Result<string>>
    {
        public Guid ReservationId { get; set; }
    }

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, Result<string>>
    {
        private readonly IReservationService _reservationService;
        public DeleteReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public async Task<Result<string>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            return await _reservationService.CancelReservation(request.ReservationId);
        }
    }
}
