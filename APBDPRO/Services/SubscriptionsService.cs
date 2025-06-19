using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Services;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly DatabaseContext _context;

    public SubscriptionsService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task AddSubscriptionAsync(AddSubscriptionDto addSubscriptionDto, CancellationToken cancellationToken)
    {
        if (addSubscriptionDto.RenewalPeriodDurationInMonths is < 1 or > 24)
            throw new BadRequestException("Renewal period duration must be between 1 and 24 months");

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


        try
        {
            var client = addSubscriptionDto.PeselOrKrs.Length switch
            {
                11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == addSubscriptionDto.PeselOrKrs,
                    cancellationToken),
                10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == addSubscriptionDto.PeselOrKrs,
                    cancellationToken),
                _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
            };

            if (client is null)
                throw new NotFoundException("Client not found");

            var software =
                await _context.Software.FirstOrDefaultAsync(e => e.Name == addSubscriptionDto.SoftwareName,
                    cancellationToken);

            if (software is null)
                throw new NotFoundException("Software not found");
            
            var price = addSubscriptionDto.Price;

            if (await _context.DiscountSoftware
                    .Where(e => e.SoftwareId == software.Id && e.Discount.DateFrom <= DateTime.Today &&
                                e.Discount.DateTo >= DateTime.Today).AnyAsync(cancellationToken))
            {
                var discount = await _context.DiscountSoftware
                    .Where(e => e.SoftwareId == software.Id && e.Discount.DateFrom <= DateTime.Today &&
                                e.Discount.DateTo >= DateTime.Today).DefaultIfEmpty()
                    .MaxAsync(e => e.Discount.Value, cancellationToken);

                price = price * (1 - (double)discount / 100);
            }

            var checkPreviousAgreement =
                await _context.Agreements.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);

            var checkPreviousSubscription =
                await _context.Subscriptions.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);

            if (checkPreviousAgreement || checkPreviousSubscription)
                price = price * 0.95;

            var offer = new Offer()
            {
                ClientId = client.Id,
                SoftwareId = software.Id,
                Price = price,
            };

            await _context.AddAsync(offer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var sub = new Subscription
            {
                OfferId = offer.Id,
                PriceForFirstInstallment = price,
                RenewalPeriodDurationInMonths = addSubscriptionDto.RenewalPeriodDurationInMonths,
                Name = addSubscriptionDto.Name,
            };

            await _context.Subscriptions.AddAsync(sub, cancellationToken);
            
            var firstPayment = new Payment
            {
                
                Amount = price,
                OfferId = offer.Id,
                PaymentDate = DateTime.Today,
                Refunded = false
            };
            await _context.Payments.AddAsync(firstPayment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task PaySubscriptionAsync(PaySubscriptionDto paySubscriptionDto, CancellationToken cancellationToken)
    {
        var sub = await _context.Subscriptions.Include(e => e.Offer).Include(e => e.StatusSubscription)
            .FirstOrDefaultAsync(e => e.Name == paySubscriptionDto.SubscriptionName, cancellationToken);
        if (sub is null)
            throw new NotFoundException("Subscription not found");
        var lastPayment = await _context.Payments.Where(e => e.OfferId == sub.OfferId).DefaultIfEmpty()
            .MaxAsync(e => e.PaymentDate, cancellationToken);

        if ((DateTime.Today - lastPayment.Date).TotalDays < sub.RenewalPeriodDurationInMonths * 30)
        {
            throw new BadRequestException("Subscription is already payed");
        }
        else if ((DateTime.Today - lastPayment.Date).TotalDays < sub.RenewalPeriodDurationInMonths * 30 * 2)
        {
            sub.StatusSubscriptionId = 2;
            throw new BadRequestException("Subscription is canceled");
        }

        var price = paySubscriptionDto.Amount;

        var checkPreviousAgreement =
            await _context.Agreements.AnyAsync(e => e.Offer.ClientId == sub.Offer.ClientId, cancellationToken);

        var checkPreviousSubscription =
            await _context.Subscriptions.AnyAsync(
                e => e.Offer.ClientId == sub.Offer.ClientId && e.OfferId != sub.OfferId, cancellationToken);

        if (checkPreviousAgreement || checkPreviousSubscription)
            price = price * 0.95;


        if (price != sub.Offer.Price)
            throw new BadRequestException("Payment amount is incorrect");

        var pay = new Payment()
        {
            Amount = price,
            OfferId = sub.OfferId,
            PaymentDate = DateTime.Today
        };

        await _context.Payments.AddAsync(pay, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}