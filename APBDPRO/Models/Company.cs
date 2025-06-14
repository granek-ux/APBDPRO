using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;


[Table("Company")]
public class Company
{
    [Key]
    [ForeignKey(nameof(Client))]
    public int Id { get; set; }
    [MaxLength(50)] public string Name { get; set; } = null!;
    [StringLength(10)]
    public string KRS { get; set; }
    
    public Client Client { get; set; } = null!;
}