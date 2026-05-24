using System.ComponentModel.DataAnnotations;
using SalvageCore.Enums;

namespace SalvageCore.Models;

public class SystemUser
{
    public Guid SystemUserId { get; set; } = Guid.CreateVersion7();

    [MaxLength(255)] public string Username { get; set; } = string.Empty;

    [MaxLength(255)] public string Number { get; set; } = string.Empty;

    [EmailAddress][MaxLength(255)] public string Email { get; set; } = string.Empty;

    [Phone][MaxLength(20)] public string Phone { get; set; } = string.Empty;

    [MaxLength(5550)] public string Address { get; set; } = string.Empty;

    [MaxLength(255)] public string Role { get; set; } = string.Empty;

    public bool AccountVerified { get; set; } = false;
    public StatusEnums Status { get; set; } = StatusEnums.Active;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}