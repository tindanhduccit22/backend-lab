namespace Lab5.Models;

public class User
{
    public int UserId { get; set; }

    public string Fullname { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public int Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? UserCode { get; set; }

    public bool IsLocked { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; } = true;

    public string? ActiveCode { get; set; }
    public string? Avatar { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}