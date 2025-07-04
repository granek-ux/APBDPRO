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
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("Person")]
        public async Task<IActionResult> AddPersonAsync([FromBody] PersonDto person,
            CancellationToken cancellationToken)
        {
            await _clientService.AddPersonAsync(person, cancellationToken);
            return Created();
        }

        [HttpDelete("Person/{pesel}")]
        public async Task<IActionResult> DeletePersonAsync(string pesel, CancellationToken cancellationToken)
        {
            await _clientService.DeletePersonAsync(pesel, cancellationToken);
            return Ok();
        }

        [HttpPut("Person/{pesel}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPerson(string pesel, [FromBody] PersonEditDto person,
            CancellationToken cancellationToken)
        {
            await _clientService.EditPersonAsync(pesel, person, cancellationToken);
            return Ok();
        }

        [HttpPost("Company")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] CompanyDto company,
            CancellationToken cancellationToken)
        {
            await _clientService.AddCompanyAsync(company, cancellationToken);
            return Created();
        }

        [HttpPut("Comoany/{KRS}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCompany(string KRS, [FromBody] CompanyEditDto company,
            CancellationToken cancellationToken)
        {
            await _clientService.EditCompanyAsync(KRS, company, cancellationToken);
            return Ok();
        }
    }
}