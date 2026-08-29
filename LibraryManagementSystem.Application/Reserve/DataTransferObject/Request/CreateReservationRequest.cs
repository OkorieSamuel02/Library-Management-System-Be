using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Reserve.DataTransferObject.Request
{
    public class CreateReservationRequest
    {
        public string Isbn { get; set; } = string.Empty;    
        public string memberEmail { get; set; } = string.Empty;
    }
}
