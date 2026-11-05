using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fruit.Models;

public partial class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
}
