using System;
using System.Collections.Generic;

namespace fruit.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int FruitId { get; set; }

    public int Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Fruit Fruit { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
