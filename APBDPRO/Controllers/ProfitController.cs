using APBDPRO.Exceptions;
using APBDPRO.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDPRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfitController : ControllerBase
    {
        private readonly IProfitService _profitService;

        public ProfitController(IProfitService profitService)
        {
            _profitService = profitService;
        }

        [HttpGet("Real/Firm")]
        public async Task<IActionResult> GetRealFirm(CancellationToken cancellationToken,string waluta = "pl")
        {
            return Ok( await _profitService.GetAllProfit(waluta,cancellationToken));
        }

        [HttpGet("Real/Product/{Name}")]
        public async Task<IActionResult> GetRealProduct(string name,CancellationToken cancellationToken,string waluta = "pl")
        {
            try
            {
                return Ok(await _profitService.GetProductProfit(waluta,name,cancellationToken));
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        
        [HttpGet("Predicted/Firm")]
        public async Task<IActionResult> GetPredictedFirm(CancellationToken cancellationToken,string waluta = "pl")
        {
            return Ok( await _profitService.GetAllProfitPredicted(waluta,cancellationToken));
        }

        [HttpGet("Predicted/Product/{Name}")]
        public async Task<IActionResult> GetPredictedProduct(string name,CancellationToken cancellationToken,string waluta = "pl")
        {
            try
            {
                return Ok(await _profitService.GetProductProfitPredicted(waluta,name,cancellationToken));
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
