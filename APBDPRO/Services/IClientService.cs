using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public interface IClientService
{
    public Task AddPersonAsync(PersonDto personDto, CancellationToken cancellationToken);
    public Task DeletePersonAsync(string pesel, CancellationToken cancellationToken);
    public Task EditPersonAsync(string pesel, PersonEditDto personDto, CancellationToken cancellationToken);
    public Task AddCompanyAsync(CompanyDto companyDto, CancellationToken cancellationToken);
    public Task EditCompanyAsync(string krs, CompanyEditDto companyDto, CancellationToken cancellationToken);
}