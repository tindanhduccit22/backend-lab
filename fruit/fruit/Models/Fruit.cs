using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fruit.Models;

public partial class Fruit
{
    public int Id { get; set; }


    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;


    [Required]
    [Range(0.01,double.MaxValue)]
    public decimal Price { get; set; }

    public double? StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
