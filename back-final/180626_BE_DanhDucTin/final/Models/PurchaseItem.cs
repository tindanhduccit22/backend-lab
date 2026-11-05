using System;
using System.Collections.Generic;

namespace final.Models;

public partial class PurchaseItem
{
    public int PurchaseItemId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int PetId { get; set; }

    public int? PurchaseItemQuantity { get; set; }

    public decimal? PurchaseItemUnitPirce { get; set; }

    public DateTime PurchaseItemCreatedAt { get; set; }

    public virtual Pet Pet { get; set; } = null!;

    public virtual PetCategory PurchaseOrder { get; set; } = null!;
}
