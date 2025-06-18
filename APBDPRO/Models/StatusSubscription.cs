using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;
[Table("Status_Subscription")]
public class StatusSubscription
{
    [Key] public int Id { get; set; }
    [MaxLength(30)] public string Name { get; set; } = null!;
    
    public ICollection<Subscription> Subscriptions { get; set; } = null!;
}