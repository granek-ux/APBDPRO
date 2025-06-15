using APBDPRO.Models.Dtos;
using APBDPRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDPRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateAgreement([FromBody] AddAgreementDto addAgreementDto, CancellationToken cancellationToken)
        {
            await _contractService.AddAgreementAsync(addAgreementDto, cancellationToken);
            return Ok();
        }
    }
}
