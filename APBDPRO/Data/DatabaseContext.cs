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


        modelBuilder.Entity<Client>().HasData(new List<Client>
        {
            new Client() { Id = 1, Adres = "Street 1", Email = "client1@example.com", PhoneNumber = 111111111 },
            new Client() { Id = 2, Adres = "Street 2", Email = "client2@example.com", PhoneNumber = 222222222 },
            new Client() { Id = 3, Adres = "Street 3", Email = "client3@example.com", PhoneNumber = 333333333 }
        });

        modelBuilder.Entity<Company>().HasData(new List<Company>
        {
            new Company() { Id = 1, Name = "Company A", KRS = "1234567891" },
            new Company() { Id = 2, Name = "Company B", KRS = "4323552342" },
            new Company() { Id = 3, Name = "Company C", KRS = "2754633906" }
        });

        modelBuilder.Entity<Person>().HasData(new List<Person>
        {
            new Person() { Id = 1, FirstName = "John", LastName = "Doe", PESEL = "12345678901" },
            new Person() { Id = 2, FirstName = "Jane", LastName = "Smith", PESEL = "23456789012" },
            new Person() { Id = 3, FirstName = "Alice", LastName = "Brown", PESEL = "34567890123" }
        });
        

        modelBuilder.Entity<SoftwareCategory>().HasData(new List<SoftwareCategory>
        {
            new SoftwareCategory() { Id = 1, Name = "Security" },
            new SoftwareCategory() { Id = 2, Name = "Office" },
            new SoftwareCategory() { Id = 3, Name = "Development" }
        });

        modelBuilder.Entity<Software>().HasData(new List<Software>
        {
            new Software()
            {
                Id = 1, Name = "Antivirus", Description = "Protects from viruses", ActualVersion = "1.0",
                SoftwareCategoryId = 1
            },
            new Software()
            {
                Id = 2, Name = "Word Processor", Description = "For documents", ActualVersion = "2.1",
                SoftwareCategoryId = 2
            },
            new Software()
                { Id = 3, Name = "IDE", Description = "For coding", ActualVersion = "3.3", SoftwareCategoryId = 3 }
        });

        modelBuilder.Entity<Discount>().HasData(new List<Discount>
        {
            new Discount()
            {
                Id = 1, Name = "New Year", Value = 10, DateFrom = DateTime.Parse("2025-01-01"),
                DateTo = DateTime.Parse("2025-01-10")
            },
            new Discount()
            {
                Id = 2, Name = "Summer Sale", Value = 15, DateFrom = DateTime.Parse("2025-07-01"),
                DateTo = DateTime.Parse("2025-07-31")
            },
            new Discount()
            {
                Id = 3, Name = "Black Friday", Value = 25, DateFrom = DateTime.Parse("2025-11-25"),
                DateTo = DateTime.Parse("2025-11-29")
            }
        });

        modelBuilder.Entity<DiscountSoftware>().HasData(new List<DiscountSoftware>
        {
            new DiscountSoftware() { SoftwareId = 1, DiscountsId = 1 },
            new DiscountSoftware() { SoftwareId = 2, DiscountsId = 2 },
            new DiscountSoftware() { SoftwareId = 3, DiscountsId = 3 }
        });
    }
}