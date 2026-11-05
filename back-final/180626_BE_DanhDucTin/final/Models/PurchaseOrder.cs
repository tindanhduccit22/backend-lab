using System;
using System.Collections.Generic;

namespace final.Models;

public partial class PurchaseOrder
{
    public int PurchaseOrderField { get; set; }

    public int UserId { get; set; }

    public DateTime PurchaseOrderOrderDate { get; set; }

    public decimal? PurchaseOrderTotalAmount { get; set; }

    public string? PurchaseOrdeStatus { get; set; }

    public DateTime PurchaseOrderCreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
