using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    [HttpGet("top-borrowed")]
    [Authorize(Policy = "AdminOrLibrarian")]
    public IActionResult GetTopBorrowedBooks()
    {
        return Ok("Top borrowed books report.");
    }
}