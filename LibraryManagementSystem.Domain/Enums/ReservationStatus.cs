using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Enums
{
    public enum ReservationStatus
    {
        Pending,
        Held,
        Fulfilled,
        Cancelled,
        Expired
    }
}
