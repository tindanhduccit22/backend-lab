using Lab4.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    [Table("Loans")]
    public class Loans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        
        public DateTime? ReturnDate { get; set; }

        [Required]
        public int Status { get; set; } // 0: Active, 1: Returned, 2: Overdue



        //  (Quan hệ khóa ngoại) 
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}