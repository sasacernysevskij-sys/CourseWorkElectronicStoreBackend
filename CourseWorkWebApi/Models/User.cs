using System.ComponentModel.DataAnnotations;
public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    [MinLength(8)]
    public string UserName { get; set; }
    [MaxLength(300)]
    public string PasswordHash { get; set; }

    [MaxLength(20)]
    public string Role { get; set; }

    public DateTime RegisterDate { get; set; }

    public DateTime UserBornDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";
}