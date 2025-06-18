using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Models;

[Table("Agreements")]
public class Agreement
{    
    [Key]
    [ForeignKey(nameof(Offer))]
    public int OfferId { get; set; }
    [ForeignKey(nameof(Client))]

    [MaxLength(100)]
    public string SoftwareVersion { get; set; } = null!;
    
    public int YearsOfAssistance {get; set;} 
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCanceled { get; set; }
    public bool IsSigned { get; set; }
    
    public Offer Offer { get; set; } = null!;
    
    
}