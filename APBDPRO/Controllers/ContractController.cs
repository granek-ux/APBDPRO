using APBDPRO.Exceptions;
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
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateAgreement([FromBody] AddAgreementDto addAgreementDto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _contractService.AddAgreementAsync(addAgreementDto, cancellationToken);
                return Ok();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem("Unexpected error occurred", statusCode: 500);
            }
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> PayAgreement([FromBody] PayAgreementDto payAgreementDto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _contractService.PayAgreementAsync(payAgreementDto, cancellationToken);
                return Ok();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem("Unexpected error occurred", statusCode: 500);
            }
        }
    }
}