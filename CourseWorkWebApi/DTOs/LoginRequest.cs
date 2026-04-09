using System.ComponentModel.DataAnnotations;
public class LoginRequest
{
    [MaxLength(50)]
    public string UserName { get; set; }
    [MaxLength(20)]
    public string Password { get; set; }
}