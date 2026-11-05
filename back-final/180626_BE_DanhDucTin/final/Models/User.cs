using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace final.Models;

public partial class User
{
    public int UserId { get; set; }
    [Required]
    [StringLength(200)]
    public string Fullname { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Required]
    [RegularExpression("Admin|Customer")]
    public string Role { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
