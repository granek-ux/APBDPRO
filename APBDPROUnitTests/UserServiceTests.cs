using APBDPRO.Data;
using APBDPRO.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBDPROUnitTests;

public class UserTests
{
    private readonly DatabaseContext _context;

    public UserTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        _context = new DatabaseContext(options);

        SeedData(); 
    }

    private void SeedData()
    {
        var role = new UserRole
        {
            Id = 1,
            Name = "Admin",
            Users = new List<User>()
        };

        var user = new User
        {
            IdUser = 1,
            Login = "testuser",
            Password = "hashedpassword",
            Salt = "salt123",
            RefreshToken = "token123",
            RefreshTokenExp = DateTime.UtcNow.AddDays(7),
            UserRoleId = 1,
            UserRole = role
        };

        role.Users.Add(user);

        _context.UserRoles.Add(role);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public void CanInsertUser()
    {
        var user = new User
        {
            IdUser = 2,
            Login = "newuser",
            Password = "pass",
            Salt = "salt",
            RefreshToken = "refresh",
            RefreshTokenExp = DateTime.UtcNow.AddDays(1),
            UserRoleId = 1
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var inserted = _context.Users.Find(2);
        Assert.NotNull(inserted);
        Assert.Equal("newuser", inserted.Login);
    }

    [Fact]
    public void UserHasCorrectRole()
    {
        var user = _context.Users.Include(u => u.UserRole).First(u => u.IdUser == 1);

        Assert.NotNull(user);
        Assert.Equal("Admin", user.UserRole.Name);
    }

    [Fact]
    public void RoleHasUsers()
    {
        var role = _context.UserRoles.Include(r => r.Users).First(r => r.Id == 1);

        Assert.NotNull(role);
        Assert.Single(role.Users);
        Assert.Equal("testuser", role.Users.First().Login);
    }
}
