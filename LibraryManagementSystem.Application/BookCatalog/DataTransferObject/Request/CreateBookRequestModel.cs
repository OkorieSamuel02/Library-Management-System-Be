using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Book.DataTransferObject.Request
{
    public class CreateBookRequestModel
    {
        public string title { get; set; } = string.Empty;
        public string author { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string genre { get; set; } = string.Empty;
        public int numberOfCopies { get; set; }
    }
}
