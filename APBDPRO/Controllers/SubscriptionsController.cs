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
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsService _subscriptionsService;

        public SubscriptionsController(ISubscriptionsService subscriptionsService)
        {
            _subscriptionsService = subscriptionsService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddSubscriptionDto addSubscriptionDto,
            CancellationToken cancellationToken)
        {
            await _subscriptionsService.AddSubscriptionAsync(addSubscriptionDto, cancellationToken);
            return Ok();
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> Pay([FromBody] PaySubscriptionDto paySubscriptionDto,
            CancellationToken cancellationToken)
        {
            await _subscriptionsService.PaySubscriptionAsync(paySubscriptionDto, cancellationToken);
            return Ok();
        }
    }
}