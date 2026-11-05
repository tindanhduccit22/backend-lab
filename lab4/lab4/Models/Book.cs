using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string BookCode { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishedYear { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Avatar { get; set; }

        public string Pdf { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public Category? Category { get; set; }
        public Author? Author { get; set; }
        public ICollection<Loan>? Loans { get; set; }
    }
}