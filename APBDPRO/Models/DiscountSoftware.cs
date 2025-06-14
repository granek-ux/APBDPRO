using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Models;

[Table("Discount_Software")]
[PrimaryKey(nameof(SoftwareId),nameof(DiscountsId))]
public class DiscountSoftware
{
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    [ForeignKey(nameof(Discount))]
    public int DiscountsId { get; set; }
    
    public Software Software { get; set; } = null!;
    public Discount Discount { get; set; } = null!;
}