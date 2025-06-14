using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("SellTypes")]
public class SellType
{
    [Key] public int Id { get; set; }
    [MaxLength(20)] public string Name { get; set; } = null!;
    
    
    public ICollection<Sell> Sells { get; set; } = null!;
}