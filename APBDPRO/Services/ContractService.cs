using APBD25_CW11.Exceptions;
using APBDPRO.Data;
using APBDPRO.Models.Dtos;

namespace APBDPRO.Services;

public class ContractService: IContractService
{
    private readonly DatabaseContext _context;

    public ContractService(DatabaseContext context)
    {
        _context = context;
    }
    
    // Tworzenie umowy:
    // Najpierw tworzymy umowę dla klienta. Umowa ma datę rozpoczęcia i zakończenia. Przedział czasowy powinien wynosić co najmniej 3 dni i maksymalnie 30 dni.
    //     Umowa musi być opłacona przez klienta we wspomnianym przedziale czasowym. W przeciwnym razie oferta jest już nieaktywna.
    //     Umowa ma również cenę z nią związaną. Cena obejmuje wszystkie możliwe zniżki.
    //     Umowa zawiera informacje o aktualizacjach, które klient może otrzymać dla zakupionego oprogramowania.
    //     Każda umowa zapewnia co najmniej 1 rok aktualizacji dla nabywcy produktu. Możemy oferować dodatkowe wsparcie w ramach umowy - każdy dodatkowy rok dodaje dodatkowe 1000 PLN do kosztu umowy.
    //     Możemy przedłużyć wsparcie tylko o 1, 2 lub 3 lata,
    // Gdy dostępnych jest wiele zniżek, wybieramy najwyższą.
    //     Wszyscy poprzedni klienci naszej firmy otrzymują 5% zniżki dla powracających klientów. Poprzedni klient oznacza, że klient zakupił co najmniej jedną subskrypcję lub podpisał jedną umowę. Ta zniżka może być dodana do innych.
    //     Umowa jest związana z wybranym oprogramowaniem i konkretną wersją tego oprogramowania.
    //     Każda umowa określa wersję oprogramowania z nią związaną.
    //     Umowa nie może być zmieniona po jej stworzeniu; może być tylko usunięta.
    //     Tworzenie umowy nie oznacza, że klient ją podpisał.
    //     Cena na umowie nie może być traktowana jako przychód. Dopiero po pełnym uregulowaniu płatności możemy traktować wartość umowy jako przychód.
    //     Tworząc umowę, upewnij się, że klient nie ma już aktywnej subskrypcji ani aktywnej umowy na ten produkt.
    public Task AddAgreementAsync(AddAgreementDto addAgreementDto, CancellationToken cancellationToken)
    {
        if (addAgreementDto.EndDate - addAgreementDto.StartDate < TimeSpan.FromDays(3))
            throw new BadRequestException("Agreement must have at least 3 days");
        if (addAgreementDto.EndDate - addAgreementDto.StartDate > TimeSpan.FromDays(30))
            throw new BadRequestException("Agreement cannot have more than 30 days");
        //todo reszta kropke str 4 od 2 kropki
        throw new NotImplementedException();
    }
}