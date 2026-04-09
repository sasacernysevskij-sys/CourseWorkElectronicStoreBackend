using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost("register")]//1
    public IActionResult Register(RegisterRequest request)
    {
        if (_db.Users.Any(u => u.UserName == request.UserName))
            return BadRequest("User exists");
        var user = new User
        {
            UserName = request.UserName,
            Role = "User",
            RegisterDate = DateTime.UtcNow,
            UserBornDate = request.UserBornDate,
            Status = "Active"
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        _db.Users.Add(user);
        _db.SaveChanges();
        return Ok();
    }
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)//8
    {
        var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);
        if (user == null)
            return Unauthorized();
        if (user.Status == "Inactive")
            return Unauthorized("You are banned");
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();
        var token = _jwt.GenerateToken(user.Id ,user.UserName, user.Role);
        return Ok(new
        {
            token,
            userName = user.UserName,
            bornDate = user.UserBornDate,
            role = user.Role
        });
    }
    [Authorize]
    [HttpPost("change-password")]
    public IActionResult ChangePassword(ChangePasswordRequest request)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (userIdClaim == null)
            return Unauthorized("UserId not found in token");
        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid UserId in token");
        var user = _db.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound("User not found");
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
        if (result == PasswordVerificationResult.Failed)
            return BadRequest("Old password is incorrect");
        user.PasswordHash = _hasher.HashPassword(user, request.NewPassword);
        _db.SaveChanges();
        return Ok("Password changed successfully");
    }
}