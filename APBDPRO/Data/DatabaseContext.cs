using APBDPRO.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DiscountSoftware> DiscountSoftware { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Sell> Sells { get; set; }
    public DbSet<SellType> SellTypes { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<SoftwareCategory> SoftwareCategories { get; set; }
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
            new UserRole() { Id = 1, Name = "User" }
        });
    }
}