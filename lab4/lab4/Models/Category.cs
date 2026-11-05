using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public string Avatar { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}