using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Helpers;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Services;

public class ProfitService : IProfitService
{
    private readonly DatabaseContext _context;
    private readonly ICurrencyHelper _currencyHelper;

    public ProfitService(DatabaseContext context, ICurrencyHelper currencyHelper)
    {
        _context = context;
        _currencyHelper = currencyHelper;
    }

    public async Task<double> GetAllProfit(string currency, CancellationToken cancellationToken)
    {
        var payments = await _context.Payments.Where(e => e.Offer.Subscription != null && !e.Refunded)
            .ToListAsync(cancellationToken);

        payments.AddRange(await _context.Payments
            .Where(e => !e.Refunded && e.Offer.Agreement != null && e.Offer.Agreement.IsSigned)
            .ToListAsync(cancellationToken));

        var profitInPln = payments.Sum(p => p.Amount);
        return await _currencyHelper.ChangeCurrency(profitInPln, currency);
    }

    public async Task<double> GetProductProfit(string currency, string softwareName,
        CancellationToken cancellationToken)
    {
        var software = await _context.Software.FirstOrDefaultAsync(e => e.Name == softwareName, cancellationToken);
        if (software is null)
            throw new NotFoundException("Software not found");

        var payments = await _context.Payments
            .Where(e => e.Offer.Subscription != null && !e.Refunded && e.Offer.SoftwareId == software.Id)
            .ToListAsync(cancellationToken);

        payments.AddRange(await _context.Payments
            .Where(e => !e.Refunded && e.Offer.Agreement != null && e.Offer.Agreement.IsSigned &&
                        e.Offer.SoftwareId == software.Id).ToListAsync(cancellationToken));

        var profit = payments.Sum(p => p.Amount);
        return await _currencyHelper.ChangeCurrency(profit, currency);
    }

    public async Task<double> GetAllProfitPredicted(string currency, CancellationToken cancellationToken)
    {
        var pay = await _context.Payments
            .Where(e => !e.Refunded && e.Offer.Agreement != null && e.Offer.Agreement.IsSigned)
            .SumAsync(e => e.Amount, cancellationToken);
        pay += await _context.Agreements.Where(e => !e.IsSigned && !e.IsCanceled && e.EndDate >= DateTime.Now)
            .SumAsync(e => e.Offer.Price, cancellationToken);
        pay += await _context.Payments.Where(e => !e.Refunded && e.Offer.Subscription != null)
            .SumAsync(e => e.Amount, cancellationToken);

        var activeSubscriptions = await _context.Subscriptions
            .Include(s => s.Offer)
            .Where(s => s.StatusSubscriptionId == 1)
            .ToListAsync(cancellationToken);

        pay += activeSubscriptions.Sum(s => s.Offer.Price); // 1 więcje

        return await _currencyHelper.ChangeCurrency(pay, currency);
    }

    public async Task<double> GetProductProfitPredicted(string currency, string softwareName,
        CancellationToken cancellationToken)
    {
        var software = await _context.Software.FirstOrDefaultAsync(e => e.Name == softwareName, cancellationToken);
        if (software is null)
            throw new NotFoundException("Software not found");

        var pay = await _context.Payments
            .Where(e => !e.Refunded && e.Offer.Agreement != null && e.Offer.Agreement.IsSigned &&
                        e.Offer.SoftwareId == software.Id).SumAsync(e => e.Amount, cancellationToken);
        pay += await _context.Agreements.Where(e =>
                !e.IsSigned && !e.IsCanceled && e.Offer.SoftwareId == software.Id && !e.IsCanceled &&
                e.EndDate >= DateTime.Now)
            .SumAsync(e => e.Offer.Price, cancellationToken);
        pay += await _context.Payments
            .Where(e => !e.Refunded && e.Offer.Subscription != null && e.Offer.SoftwareId == software.Id)
            .SumAsync(e => e.Amount, cancellationToken);

        var activeSubscriptions = await _context.Subscriptions
            .Include(s => s.Offer)
            .Where(e => e.StatusSubscriptionId == 1 && e.Offer.SoftwareId == software.Id)
            .ToListAsync(cancellationToken);

        pay += activeSubscriptions.Sum(s => s.Offer.Price); // 1 więcje

        return await _currencyHelper.ChangeCurrency(pay, currency);
    }
}