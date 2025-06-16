using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Models;

[Table("Agreements")]
public class Agreement
{    
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    [ForeignKey(nameof(Software))]
    public int SoftwareId { get; set; }

    [MaxLength(100)]
    public string SoftwareVersion { get; set; } = null!;
    
    public int YearsOfAssistance {get; set;} 
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPaid { get; set; }
    public double Price { get; set; }
    public bool IsSigned { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = null!;

    
    public Client Client { get; set; } = null!;
    public Software Software { get; set; } = null!;
    
    
}