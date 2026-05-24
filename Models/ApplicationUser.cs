using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SalvageCore.Models;

public class ApplicationUser : IdentityUser
{
    public Guid SystemUserId { get; set; }
    public virtual SystemUser? SystemUser { get; set; }

    [MaxLength(500)] public string? Domain { get; set; } = string.Empty;
}