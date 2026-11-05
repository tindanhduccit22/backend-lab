using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fruit.Models;

public partial class User
{
    public int Id { get; set; }
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Required]
    [RegularExpression("Admin|User",
    ErrorMessage = "Role must be Admin or User")]
    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
