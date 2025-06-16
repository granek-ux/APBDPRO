using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Offers")]
public class Offer
{
    [Key]
    public int Id { get; set; }
    public double Price { get; set; }
    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    
    public Client Client { get; set; } = null!;
    public Software Software { get; set; } = null!;
    
    public ICollection<Payment> Payments { get; set; } = null!;
    
    public Agreement? Agreement { get; set; }
    public Subscription? Subscription { get; set; }
    
}