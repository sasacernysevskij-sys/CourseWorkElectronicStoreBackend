
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public string RegisterDate { get; set; }
    public string UserBornDate { get; set; }
    public string Status { get; set; } = "Active";
}