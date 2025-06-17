using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Payment")]
public class Payment
{
    [Key] public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public double Amount { get; set; }
    [ForeignKey(nameof(Offer))]
    public int OfferId { get; set; }
    
    public bool Refunded { get; set; } = false;
    
    public Offer Offer { get; set; } =  null!;
    
    
}