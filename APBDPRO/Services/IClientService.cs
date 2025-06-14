using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public interface IClientService
{
    public Task AddPersonAsync(PersonDto personDto, CancellationToken cancellationToken);
    public Task DeletePersonAsync(string pesel, CancellationToken cancellationToken);
    public Task AddCompanyAsync(CompanyDto companyDto, CancellationToken cancellationToken);
}