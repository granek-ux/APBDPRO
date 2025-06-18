using System.Text.Json;
using APBDPRO.Models;

namespace APBDPRO.Helpers;

public class CurrencyHelper : ICurrencyHelper
{
    private readonly HttpClient _httpClient;

    public CurrencyHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<double> ChangeCurrency(double amount, string currency)
    {
        if(currency.ToLower().Equals("pl"))
            return amount;
        var response = await _httpClient.GetAsync($"https://api.nbp.pl/api/exchangerates/rates/A/{currency}/");
        
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return data.Rates[0].Mid*amount;
    }
}
