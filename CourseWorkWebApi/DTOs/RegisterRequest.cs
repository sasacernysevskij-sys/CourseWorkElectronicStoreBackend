using System.ComponentModel.DataAnnotations;
public class RegisterRequest
{
    [MaxLength(50)]
    public string UserName { get; set; }
    [MaxLength(50)]
    public string Password { get; set; }
    public DateTime UserBornDate { get; set; }
}