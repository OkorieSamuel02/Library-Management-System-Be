using LibraryManagementSystem.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve.Command
{
    public class ClaimReservationCommand : IRequest<Result<string>>
    {
        public Guid ReservationId { get; set; }
    }

    public class ClaimReservationCommandHandler : IRequestHandler<ClaimReservationCommand, Result<string>>
    {
        private readonly IReservationService _reservationService;
        public ClaimReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public async Task<Result<string>> Handle(ClaimReservationCommand request, CancellationToken cancellationToken)
        {
            return await _reservationService.ClaimReservation(request.ReservationId);
        }
    }

}
