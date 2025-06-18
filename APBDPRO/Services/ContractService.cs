using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Services;

public class ContractService : IContractService
{
    private readonly DatabaseContext _context;

    public ContractService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddAgreementAsync(AddAgreementDto addAgreementDto, CancellationToken cancellationToken)
    {
        if (addAgreementDto.EndDate - addAgreementDto.StartDate < TimeSpan.FromDays(3))
            throw new BadRequestException("Agreement must have at least 3 days");
        if (addAgreementDto.EndDate - addAgreementDto.StartDate > TimeSpan.FromDays(30))
            throw new BadRequestException("Agreement cannot have more than 30 days");

        if (addAgreementDto.HowMuchLongerAssistance > 3)
            throw new BadRequestException("Agreement assistance cannot be extended for more than 3 years");
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


        try
        {
            var client = addAgreementDto.PeselOrKrs.Length switch
            {
                11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == addAgreementDto.PeselOrKrs,
                    cancellationToken),
                10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == addAgreementDto.PeselOrKrs,
                    cancellationToken),
                _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
            };

            if (client is null)
                throw new NoMatchFoundException("Client not found");

            var software =
                await _context.Software.FirstOrDefaultAsync(e => e.Name == addAgreementDto.SoftwareName,
                    cancellationToken);

            if (software is null)
                throw new NotFoundException("Software not found");

            var checkAgreement = await _context.Agreements.FirstOrDefaultAsync(
                e => e.Offer.ClientId == client.Id && e.Offer.SoftwareId == software.Id && e.EndDate >= DateTime.Today,
                cancellationToken);

            if (checkAgreement != null)
                throw new ConflictException("Agreement already exists");

            var checkPreviousAgreement =
                await _context.Agreements.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);

            var checkPreviousSubscription =
                await _context.Subscriptions.AnyAsync(e => e.Offer.ClientId == client.Id, cancellationToken);

            var price = addAgreementDto.Price;

            price += addAgreementDto.HowMuchLongerAssistance * 1000;

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

            var agreement = new Agreement
            {
                OfferId = offer.Id,
                SoftwareVersion = software.ActualVersion,
                YearsOfAssistance = 1 + addAgreementDto.HowMuchLongerAssistance,
                StartDate = addAgreementDto.StartDate,
                EndDate = addAgreementDto.EndDate,
                IsCanceled = false,
                IsSigned = false,
            };

            await _context.Agreements.AddAsync(agreement, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task PayAgreementAsync(PayAgreementDto payAgreementDto, CancellationToken cancellationToken)
    {
        if (payAgreementDto.Amount <= 0)
            throw new BadRequestException("Amount must be greater than 0");

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


        try
        {
            var client = payAgreementDto.PeselOrKrs.Length switch
            {
                11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == payAgreementDto.PeselOrKrs,
                    cancellationToken),
                10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == payAgreementDto.PeselOrKrs,
                    cancellationToken),
                _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
            };

            if (client is null)
                throw new NoMatchFoundException("Client not found");

            var software =
                await _context.Software.FirstOrDefaultAsync(e => e.Name == payAgreementDto.SoftwareName,
                    cancellationToken);

            if (software is null)
                throw new NotFoundException("Software not found");

            var agreement = await _context
                .Agreements
                .Include(e => e.Offer).ThenInclude(e => e.Payments)
                .FirstOrDefaultAsync(
                    e => e.Offer.ClientId == client.Id && e.Offer.SoftwareId == software.Id && e.IsSigned == false &&
                         e.EndDate >= DateTime.Today, cancellationToken);


            if (agreement is null)
                throw new NotFoundException("Agreement not found");

            if (agreement.EndDate < DateTime.Today)
            {
                agreement.IsCanceled = true;
                foreach (var oldPayment in agreement.Offer.Payments)
                {
                    oldPayment.Refunded = true;
                }

                throw new ConflictException("Agreement was canceled due to too late payment");
            }

            var payedAgreement = agreement.Offer.Payments.Sum(e => e.Amount);

            var wantToPay = payAgreementDto.Amount;


            if (payedAgreement + wantToPay >= agreement.Offer.Price)
            {
                agreement.IsSigned = true;
                wantToPay = agreement.Offer.Price - payedAgreement;
            }

            var payment = new Payment
            {
                OfferId = agreement.OfferId,
                Amount = wantToPay,
                PaymentDate = DateTime.Today,
            };

            await _context.AddAsync(payment, cancellationToken);


            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}