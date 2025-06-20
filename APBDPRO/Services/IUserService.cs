using APBDPRO.Models;
using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public interface IUserService
{
    public Task RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken);

    public Task<Tuple<string, string>> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken);

    public Task<Tuple<string, string>> RefreshAsync(RefreshTokenDto refreshToken, CancellationToken cancellationToken);

    string CreateAccessTokenAsync(User user);
}