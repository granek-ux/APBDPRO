using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public interface ISubscriptionsService
{
    public Task AddSubscriptionAsync(AddSubscriptionDto  addSubscriptionDto, CancellationToken cancellationToken);
}