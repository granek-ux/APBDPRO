using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Models;

[Table("Sells")]
[PrimaryKey(nameof(ClientId), nameof(SoftwareId))]
public class Sell
{    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    [ForeignKey(nameof(SellType))]
    public int SellTypeId { get; set; }
    
    public Client Client { get; set; } = null!;
    public Software Software { get; set; } = null!;
    public SellType SellType { get; set; } = null!;
    
    
}