using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Discounts")]
public class Discount
{
    [Key] public int Id { get; set; }
    [MaxLength(20)] public string Name { get; set; } = null!;
    public int Value { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }

    public ICollection<DiscountSoftware> DiscountSoftwares { get; set; } = null!;
}