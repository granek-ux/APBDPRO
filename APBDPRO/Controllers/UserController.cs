using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using APBDPRO.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDPRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            await _userService.RegisterUserAsync(registerDto, cancellationToken);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var info = await _userService.LoginUserAsync(loginDto, cancellationToken);
            return Ok(new { accessToken = info.Item1, refreshToken = info.Item2 });
        }

        [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshToken, CancellationToken cancellationToken)
        {
            var info = await _userService.RefreshAsync(refreshToken, cancellationToken);
            return Ok(new { accessToken = info.Item1, refreshToken = info.Item2 });
        }
    }
}