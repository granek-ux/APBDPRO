using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Subscription")]
public class Subscription
{
    [Key]
    [ForeignKey(nameof(Offer))]
    public int OfferId { get; set; }
    
    
    public int RenewalPeriodDurationInMonths {get; set;}
    
    [MaxLength(50)] public string Name { get; set; } = null!;
    
    public double PriceForFirstInstallment { get; set; }

    [ForeignKey(nameof(StatusSubscription))]
    public int StatusSubscriptionId { get; set; } = 1;
    
    public StatusSubscription StatusSubscription { get; set; } = null!;
    public Offer Offer { get; set; } = null!;
}