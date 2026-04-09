using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ElectronicStore.Controllers;
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db)
    {
        _db = db;
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _db.Users
            .Where(u => u.Status == "Active")
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.Role,
                u.RegisterDate
            })
            .ToList();
        return Ok(users);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("ban")]
    public IActionResult BanUser(int id)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        user.Status = "Inactive";
        _db.SaveChanges();
        return Ok($"User {user.UserName} banned");
    }
    [HttpGet("admin-count")]
    public IActionResult GetAdminCount()
    {
        var count = _db.Users.Count(u => u.Status == "Active" && u.Role == "Admin");
        return Ok(new { count });
    }
}