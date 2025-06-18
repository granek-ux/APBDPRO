namespace APBDPRO.Helpers;

public interface ICurrencyHelper
{
    Task<double> ChangeCurrency(double amount, string currency);
}