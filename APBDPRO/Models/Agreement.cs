using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Models;

[Table("Agreements")]
[PrimaryKey(nameof(ClientId), nameof(SoftwareId))]
public class Agreement
{    
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPaid { get; set; }
    public double Price { get; set; }
    public bool IsSigned { get; set; }

    
    public Client Client { get; set; } = null!;
    public Software Software { get; set; } = null!;
    
    
}