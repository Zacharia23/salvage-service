using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public AccountType? AccountType { get; set; }
    public Guid? IdentityTypeId { get; set; }
    public virtual IdentityType? IdentityType { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public GenderEnum? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool AcceptedTerms { get; set; } = false;
    public bool AccountVerified { get; set; } = false;
    public Guid? RegionId { get; set; }
    public virtual Region? Region { get; set; }
    public string TaxNumber { get; set; } = string.Empty;
    public string VNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<CustomerNotification> CustomerNotifications { get; set; } = new List<CustomerNotification>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}