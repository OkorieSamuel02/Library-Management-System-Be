using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementSystem.Domain.Entity
{
    public class Reservation
    {
        public Guid id { get; set; }
        public DateTime reservationDate { get; private set; } = DateTime.UtcNow;
        public ReservationStatus reservationStatus { get; private set; } = ReservationStatus.Pending;
        public DateTime? holdExpiresAt { get; private set; }
        public Guid bookId { get; set; }
        public Book books { get; set; } 
        public Guid MemberId { get; set; }
        public Member members { get; set; } 
       


        public void SetHoldExpiration(DateTime returnDate)
        {
            var getHours = returnDate.AddHours(48);
            holdExpiresAt = getHours;
        }

        public void UpdateStatus(ReservationStatus status)
        {
            reservationStatus = status;
        }

        public void CancelReservation()
        {
            UpdateStatus(ReservationStatus.Cancelled);
        }

    }
}
