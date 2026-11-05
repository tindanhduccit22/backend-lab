using Lab4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string BookCode { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishedYear { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public int TotalCopies { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string Avatar { get; set; }

        public string Pdf { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = true;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        // 1 cuốn sách có thể mượn được nhiều lần
        public virtual ICollection<Loans> Loans { get; set; } = new List<Loans>();
    }
}