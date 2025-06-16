using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public interface IContractService
{
    public Task AddAgreementAsync(AddAgreementDto addAgreementDto, CancellationToken cancellationToken);
    
    public Task PayAgreementAsync(PayAgreementDto payAgreementDto, CancellationToken cancellationToken);
}