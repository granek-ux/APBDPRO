using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Payment")]
public class Payment
{
    [Key] public int Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public double Amount { get; set; }
    [ForeignKey(nameof(Agreement))]
    public int AgreementId { get; set; }
    
    public bool Refunded { get; set; } = false;
    
    public Agreement Agreement { get; set; } =  null!;
    
    
}