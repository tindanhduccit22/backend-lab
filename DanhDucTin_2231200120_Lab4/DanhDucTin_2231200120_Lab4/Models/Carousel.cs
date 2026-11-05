using Lab4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    [Table("Carousel")]
    public class Carousel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int CarouselId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }

        [Required]
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
