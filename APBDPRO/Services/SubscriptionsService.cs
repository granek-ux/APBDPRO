using APBD25_CW11.Exceptions;
using APBDPRO.Data;
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
        
        //todo Tranzact
        if (addSubscriptionDto.RenewalPeriodDurationInMonths is < 1 or > 24 )
            throw new BadRequestException("Renewal period duration must be between 1 and 24 months");
        
        var client = addSubscriptionDto.PeselOrKrs.Length switch
        {
            11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == addSubscriptionDto.PeselOrKrs,
                cancellationToken),
            10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == addSubscriptionDto.PeselOrKrs,
                cancellationToken),
            _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
        };

        if (client is null)
            throw new NoMatchFoundException("Client not found");

        var software =
            await _context.Software.FirstOrDefaultAsync(e => e.Name == addSubscriptionDto.SoftwareName,
                cancellationToken);

        if (software is null)
            throw new NotFoundException("Software not found");
        
        var discount = await _context.DiscountSoftware
            .Where(e => e.SoftwareId == software.Id && e.Discount.DateFrom <= DateTime.Today &&
                        e.Discount.DateTo >= DateTime.Today).DefaultIfEmpty()
            .MaxAsync(e => e.Discount.Value, cancellationToken);

        var price = addSubscriptionDto.Price;
        
        price = price * (1 - (double)discount / 100);
        
        var checkPreviousAgreement =
            await _context.Agreements.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);
        
        var checkPreviousSubscription = await _context.Subscriptions.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);
        
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
        await _context.SaveChangesAsync(cancellationToken);

    }

    public Task PaySubscriptionAsync(PaySubscriptionDto paySubscriptionDto, CancellationToken cancellationToken)
    {
        
        var lastPayment = _context;
        throw new NotImplementedException();
    }
}