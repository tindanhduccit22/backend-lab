using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpPost("upload")]
    [Authorize(Policy = "VerifiedEmailOnly")]
    public IActionResult UploadBook()
    {
        return Ok("Book uploaded successfully");
    }
}