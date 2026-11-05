using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int Status { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }
    }
}