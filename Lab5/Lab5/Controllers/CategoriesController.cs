using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "ManageActiveCategories")]
    public IActionResult GetCategories()
    {
        return Ok("Category Management Access Granted");
    }
}