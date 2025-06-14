using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Software_Category")]
public class SoftwareCategory
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)] public string Name { get; set; } = null!;
    
    public ICollection<Software> Softwares { get; set; } = null!;
}