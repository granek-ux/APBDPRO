using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDPRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
        
    }
}
