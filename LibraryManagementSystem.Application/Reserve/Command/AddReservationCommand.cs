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
    public class AddReservationCommand : CreateReservationRequest, IRequest<Result<string>>
    {

    }

    public class AddReservationCommandHandler : IRequestHandler<AddReservationCommand, Result<string>>
    {
        private readonly IReservationService _reservationService;
        public AddReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public async Task<Result<string>> Handle(AddReservationCommand request, CancellationToken cancellationToken)
        {
            return await _reservationService.AddedReservation(request);
        }
    }
}
