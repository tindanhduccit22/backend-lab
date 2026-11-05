using System.ComponentModel.DataAnnotations;

namespace Lab4.DTOs 
{
    public class CreateLoanDto
    {
        [Required(ErrorMessage = "User ID cannot be left blank.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The book code field cannot be left blank.")]
        public int BookId { get; set; }
    }
}