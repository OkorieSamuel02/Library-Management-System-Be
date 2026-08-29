using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;
using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve.DataTransferObject.Response
{
    public class GetReservationResponseModel
    {
        public Guid id { get; set; }
        public DateTime reservationDate { get;  set; } 
        public string? reservationStatus { get;  set; } 
        public DateTime? holdExpiresAt { get;  set; }
        public BookResponseModel? books { get; set; }
        public MemberResponseModel members { get; set; }
    }

    public class GetPersonalReservationResponse
    {
        public Guid id { get; set; }
        public DateTime reservationDate { get;  set; }
        public string? reservationStatus { get;  set; }
        public DateTime? holdExpiresAt { get;  set; }
        public BookResponseModel? books { get; set; }
    }
}
