using System.ComponentModel.DataAnnotations;

public class ChangePasswordRequest
{
    [MaxLength(50)]
    public string OldPassword { get; set; }
    [MaxLength(50)]
    public string NewPassword { get; set; }
}