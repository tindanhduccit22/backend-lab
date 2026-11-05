using Lab5.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var email = User.Identity?.Name;

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(r => r.Value)
            .ToList();

        return Ok(new
        {
            email,
            roles
        });
    }

    [HttpGet("active-users")]
    [Authorize(Policy = "ActiveUserOnly")]
    public IActionResult GetActiveUsers()
    {
        var users = _context.Users
            .Where(u => u.IsActive)
            .ToList();

        return Ok(users);
    }

    [HttpGet("history")]
    [Authorize(Policy = "MinimumMembership")]
    public IActionResult GetLoanHistory()
    {
        return Ok("Loan history");
    }
}