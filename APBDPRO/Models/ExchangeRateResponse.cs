namespace APBDPRO.Models;

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