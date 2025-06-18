using APBDPRO.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Agreement> Agreements { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DiscountSoftware> DiscountSoftware { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    
    public DbSet<Person> People { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<SoftwareCategory> SoftwareCategories { get; set; }
    public DbSet<StatusSubsription>  StatusSubsriptions { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>().HasData(new List<UserRole>
        {
            new UserRole() { Id = 1, Name = "User" },
            new UserRole() { Id = 2, Name = "Admin" },
        });

        modelBuilder.Entity<StatusSubsription>().HasData(new List<StatusSubsription>
        {
            new StatusSubsription() { Id = 1, Name = "Active" },
            new StatusSubsription() { Id = 2, Name = "Cancelled" },
        });

    }
}