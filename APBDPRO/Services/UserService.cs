using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Helpers;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBDPRO.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;

    public UserService(IConfiguration configuration, DatabaseContext context)
    {
        _configuration = configuration;
        _context = context;
    }


    public async Task RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(registerDto.Password);

        var user = new User()
        {
            Email = registerDto.Email,
            Login = registerDto.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.UtcNow.AddDays(1),
            UserRoleId = 1
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return;
    }

    public async Task<Tuple<string, string>> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(e=> e.UserRole ).FirstOrDefaultAsync(e => e.Login == loginDto.Login, cancellationToken);
        if (user is null)
            throw new UnauthorizedException("Invalid login or password");

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginDto.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
            throw new UnauthorizedException("Invalid login or password");

        var accessToken = CreateAccessTokenAsync(user);
        
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.UtcNow.AddDays(1);
        await _context.SaveChangesAsync(cancellationToken);
        return new Tuple<string, string>(accessToken, user.RefreshToken);
    }

    public async Task<Tuple<string, string>> RefreshAsync(RefreshTokenDto refreshToken, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(e=> e.UserRole ).FirstOrDefaultAsync(e => e.RefreshToken == refreshToken.RefreshToken, cancellationToken);
        if (user is null)
        {
            throw new SecurityException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityException("Refresh token expired");
        }
        var accessToken = CreateAccessTokenAsync(user);
        

        return new Tuple<string, string>(accessToken, user.RefreshToken);
    }

    public string CreateAccessTokenAsync(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.UserRole.Name)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SymmetricSecurityKey"]!));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials,
            Expires = DateTime.Now.AddHours(1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[96];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}