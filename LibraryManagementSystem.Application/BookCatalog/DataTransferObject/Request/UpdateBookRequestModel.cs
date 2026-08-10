using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Book.DataTransferObject.Request
{
    public class UpdateBookRequestModel
    {
        public string? title { get; set; } 
        public string? author { get; set; } 
        public string isbn { get; set; } = string.Empty;
        public string? genre { get; set; } 
        public int numberOfCopies { get; set; }
    }
}
