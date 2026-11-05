using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace final.Models;

public partial class PetCategory
{
    public int PetCategoryId { get; set; }
    [Required]
    [StringLength(50)]
    public string PetCategoryName { get; set; } = null!;

    public string? PetCategoryDescription { get; set; }

    public DateTime PetCategoryCreatedAt { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
