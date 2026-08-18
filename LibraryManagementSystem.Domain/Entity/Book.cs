using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Entity
{
    public class Book
    {
        public Guid id {  get; set; }
        public string title { get; set; } = string.Empty;
        public string author { get; set; } = string.Empty;
        public string isbn {  get; set; } = string.Empty;
        public string genre {  get; set; } = string.Empty;
        public int totalCopies { get; private set; }
        public int availableCopies { get; private set; }
        public DateTime createAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Loan>? loans { get; private set; } = new List<Loan>();   

        public void BooksUpdate(int newCopies)
        {
            var borrowedCopies = totalCopies - availableCopies;

            if(newCopies < 0)
                throw new ArgumentException("Total copies cannot be negative.");

            if (newCopies < borrowedCopies)
                throw new InvalidOperationException("Total copies cannot be less than borrowed copies.");

            totalCopies = newCopies;
            availableCopies = newCopies - borrowedCopies;

            updatedAt = DateTime.UtcNow;
        }

        public void BorrowBook()
        {

            if (availableCopies <= 0)
                throw new InvalidOperationException("Qauntity cannot be less than borrowed copies.");

            availableCopies  --;

        }

        public void BookOnCreation(int copies)
        {
            if (copies < 0)
                throw new ArgumentException("Total copies cannot be negative.");

            if(copies < availableCopies)
                throw new InvalidOperationException("Total copies cannot be less than available copies.");

            totalCopies = copies;
            availableCopies = copies;
        }

        public void ReturnBook()
        {
        
            availableCopies ++;

        }
    }
}
