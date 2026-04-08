using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ElectronicStore.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthController(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        if (_db.Users.Any(u => u.UserName == request.UserName))
            return BadRequest("User exists");
        var user = new User
        {
            UserName = request.UserName,
            Role = "User",
            RegisterDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            UserBornDate = request.UserBornDate,
            Status = "Active"
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        _db.Users.Add(user);
        _db.SaveChanges();
        return Ok();
    }
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);
        if (user == null)
            return Unauthorized();
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();
        var token = _jwt.GenerateToken(user.UserName, user.Role);
        return Ok(new
        {
            token,
            userName = user.UserName,
            bornDate = user.UserBornDate,
            role = user.Role
        });
    }
}