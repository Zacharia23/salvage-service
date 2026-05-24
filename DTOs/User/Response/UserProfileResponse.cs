namespace SalvageCore.DTOs.User.Response;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Number { get; set; }
    public string Address { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
}