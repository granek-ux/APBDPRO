using System.Text.Json;

namespace APBDPRO.Helpers;

public class CurrencyHelper
{
    public static async Task<double> ChangeCurrency(double amount, string currency)
    {
        var httpClient =  new HttpClient();
        if(currency.ToLower().Equals("pl"))
            return amount;
        var response = await httpClient.GetAsync($"https://api.nbp.pl/api/exchangerates/rates/A/{currency}/");
        
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return data.Rates[0].Mid*amount;
    }
}
public class ExchangeRateResponse
{
    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public Rate[] Rates { get; set; }

    public class Rate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public double Mid { get; set; }
    }
}