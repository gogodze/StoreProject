using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers.v1;

[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    [HttpGet("/admin/users/")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok("ww");
    }
}