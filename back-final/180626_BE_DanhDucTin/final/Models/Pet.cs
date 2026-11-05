using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace final.Models;

public partial class Pet
{
    public int PetId { get; set; }

    [Required]
    [StringLength(100)]
    public string PetName { get; set; } = null!;

    public string PetBreed { get; set; } = null!;

    public int? PetAge { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PetPrice { get; set; }

    public int PetCategoryId { get; set; }

    public int? PetQuantity { get; set; }

    public DateTime PetCreatedAt { get; set; }

    public virtual PetCategory PetCategory { get; set; } = null!;

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
