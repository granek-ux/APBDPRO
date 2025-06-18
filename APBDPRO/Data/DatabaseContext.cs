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
    public DbSet<StatusSubscription> StatusSubsriptions { get; set; }
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
            new UserRole { Id = 1, Name = "User" },
            new UserRole { Id = 2, Name = "Admin" }
        });

        modelBuilder.Entity<User>().HasData(
                new User
                {
                    IdUser = 1, Login = "admin", Password = "rrLQWg2e++4RSCGid02OYSlPuHz21DShv+H1RPrpIRk=",
                    Salt = "7FPhRi75IVLn9VcF6TMfrw==", RefreshToken = "OnVJ/FbGSyIUCpdceCuCxti7b7wrg+/+TnzGohsVdLM=",
                    RefreshTokenExp = DateTime.Now, UserRoleId = 2
                }
            
        );

        modelBuilder.Entity<StatusSubscription>().HasData(new List<StatusSubscription>
        {
            new StatusSubscription { Id = 1, Name = "Active" },
            new StatusSubscription { Id = 2, Name = "Cancelled" }
        });

        modelBuilder.Entity<Client>().HasData(new List<Client>
        {
            new Client { Id = 1, Adres = "123 Main St", Email = "client1@example.com", PhoneNumber = 123456789 },
            new Client { Id = 2, Adres = "456 Elm St", Email = "client2@example.com", PhoneNumber = 987654321 }
        });

        modelBuilder.Entity<Company>().HasData(new Company
        {
            Id = 1,
            Name = "Tech Corp",
            KRS = "1234567890"
        });

        modelBuilder.Entity<Person>().HasData(new Person
        {
            Id = 2,
            FirstName = "John",
            LastName = "Doe",
            PESEL = "90010112345",
            Deleted = false
        });

        modelBuilder.Entity<SoftwareCategory>().HasData(new List<SoftwareCategory>
        {
            new SoftwareCategory { Id = 1, Name = "Antivirus" },
            new SoftwareCategory { Id = 2, Name = "Office Suite" }
        });

        modelBuilder.Entity<Software>().HasData(new List<Software>
        {
            new Software
            {
                Id = 1,
                Name = "SafeGuard",
                Description = "Antivirus software",
                ActualVersion = "1.2.3",
                Price = 49.99,
                SoftwareCategoryId = 1
            },
            new Software
            {
                Id = 2,
                Name = "OfficeMax",
                Description = "Office productivity suite",
                ActualVersion = "2023.1",
                Price = 89.99,
                SoftwareCategoryId = 2
            }
        });

        modelBuilder.Entity<Discount>().HasData(new Discount
        {
            Id = 1,
            Name = "Spring Sale",
            Value = 20,
            DateFrom = new DateTime(2025, 3, 1),
            DateTo = new DateTime(2025, 3, 31)
        });

        modelBuilder.Entity<DiscountSoftware>().HasData(new DiscountSoftware
        {
            SoftwareId = 1,
            DiscountsId = 1
        });

        modelBuilder.Entity<Offer>().HasData(new Offer
        {
            Id = 1,
            Price = 39.99,
            ClientId = 1,
            SoftwareId = 1
        });

        modelBuilder.Entity<Agreement>().HasData(new Agreement
        {
            OfferId = 1,
            SoftwareVersion = "1.2.3",
            YearsOfAssistance = 2,
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2027, 4, 1),
            IsCanceled = false,
            IsSigned = true
        });

        modelBuilder.Entity<Subscription>().HasData(new Subscription
        {
            OfferId = 1,
            RenewalPeriodDurationInMonths = 12,
            Name = "Annual Plan",
            PriceForFirstInstallment = 29.99,
            StatusSubscriptionId = 1
        });

        modelBuilder.Entity<Payment>().HasData(new Payment
        {
            Id = 1,
            PaymentDate = new DateTime(2025, 4, 2),
            Amount = 39.99,
            OfferId = 1,
            Refunded = false
        });
    }
}