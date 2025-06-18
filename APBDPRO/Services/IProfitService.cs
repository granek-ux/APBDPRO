namespace APBDPRO.Services;

public interface IProfitService
{
    public Task<double> GetAllProfit (string currency ,CancellationToken cancellationToken);
    
    public Task<double> GetProductProfit (string currency, string softwareName, CancellationToken cancellationToken);
    
    public Task<double> GetAllProfitPredicted (string currency ,CancellationToken cancellationToken);
    
    public Task<double> GetProductProfitPredicted (string currency, string softwareName, CancellationToken cancellationToken);
}