using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Models;

namespace SalvageCore.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<SystemUser> SystemUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Make> Makes { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<IdentityType> IdentityTypes { get; set; }
    public DbSet<VehicleImage> VehicleImages { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Receipts> Receipts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<CustomerNotification> CustomerNotifications { get; set; }
    public DbSet<Questions> Questions { get; set; }
    public DbSet<WorkingInfo> WorkingInfo { get; set; }
    public DbSet<Sections> Sections { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<MessageLogs> MessageLogs { get; set; }
    public DbSet<TempCustomer> TempCustomers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Customer>()
            .HasOne(c => c.IdentityType)
            .WithMany(id => id.Customers)
            .HasForeignKey(c => c.IdentityTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Customer>()
            .HasOne(c => c.Region)
            .WithMany(id => id.Customers)
            .HasForeignKey(c => c.RegionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Bid>()
            .HasOne(b => b.Customer)
            .WithMany(id => id.Bids)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ActivityLog>()
            .HasOne(l => l.Customer)
            .WithMany(id => id.ActivityLogs)
            .HasForeignKey(l => l.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Company>()
            .HasMany(v => v.Vehicles)
            .WithOne(c => c.Company)
            .HasForeignKey(v => v.CompanyId);

        builder.Entity<Make>()
            .HasMany(n => n.Models)
            .WithOne(m => m.Make)
            .HasForeignKey(n => n.MakeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Make>()
            .HasMany(v => v.Vehicles)
            .WithOne(m => m.Make)
            .HasForeignKey(v => v.MakeId);

        builder.Entity<Model>()
            .HasMany(v => v.Vehicles)
            .WithOne(m => m.Model)
            .HasForeignKey(v => v.ModelId);

        builder.Entity<Region>()
            .HasMany(v => v.Vehicles)
            .WithOne(r => r.Region)
            .HasForeignKey(r => r.RegionId);

        builder.Entity<Vehicle>()
            .HasMany(img => img.VehicleImages)
            .WithOne(v => v.Vehicle)
            .HasForeignKey(img => img.VehicleId);

        builder.Entity<Vehicle>()
            .HasOne(o => o.Offer)
            .WithOne(v => v.Vehicle)
            .HasForeignKey<Offer>(o => o.VehicleId);

        builder.Entity<Offer>()
            .HasMany(b => b.Bids)
            .WithOne(o => o.Offer)
            .HasForeignKey(b => b.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Notification>()
            .HasOne(n => n.Customer)
            .WithMany(c => c.Notifications)
            .HasForeignKey(n => n.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CustomerNotification>()
            .HasOne(c => c.Customer)
            .WithMany(sc => sc.CustomerNotifications)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CustomerNotification>()
            .HasOne(cn => cn.Notification)
            .WithMany(n => n.CustomerNotifications)
            .HasForeignKey(n => n.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Invoice>()
            .HasOne(n => n.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(n => n.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Invoice>()
            .HasOne(c => c.Bid)
            .WithMany(b => b.Invoices)
            .HasForeignKey(c => c.BidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Subscription>()
            .HasOne(s => s.Offer)
            .WithMany(o => o.Subscriptions)
            .HasForeignKey(s => s.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Subscription>()
            .HasOne(s => s.Customer)
            .WithMany(o => o.Subscriptions)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }
}