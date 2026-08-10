using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Book.DataTransferObject.Response
{
    public class BookResponseModel
    {
        public Guid id { get; set; }
        public string title { get; set; } = string.Empty;
        public string author { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string genre { get; set; } = string.Empty;
        public int totalCopies { get; set; }
        public int availableCopies { get; set; }
        public DateTime createAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
}
