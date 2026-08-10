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
        public int totalCopies { get; set; }
        public int availableCopies { get; private set; }
        public DateTime createAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;


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

        public void BorrowBook(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");


            if (quantity > availableCopies)
                throw new InvalidOperationException("Total copies cannot be less than borrowed copies.");

            availableCopies  -= quantity;

        }
    }
}
