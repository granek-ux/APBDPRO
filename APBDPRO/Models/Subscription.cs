using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Subscription")]
public class Subscription
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    
    public int RenewalPeriodDurationInMonths {get; set;}
    
    [MaxLength(50)] public string Name { get; set; } = null!;
    
    public double Price { get; set; }
    public double PriceForFirstInstallment { get; set; }
    
    public Client Client { get; set; } = null!;
    public Software Software { get; set; } = null!;

}