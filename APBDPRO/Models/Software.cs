using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;


[Table("Software")]
public class Software
{
    public int Id { get; set; }
    [MaxLength(20)] public string Name { get; set; } = null!;
    [MaxLength(20)] public string Description { get; set; } = null!;
    [MaxLength(20)] public string ActualVersion { get; set; } = null!;
    
    [ForeignKey(nameof(SoftwareCategory))]
    public int SoftwareCategoryId { get; set; }
    
    public SoftwareCategory SoftwareCategory { get; set; } = null!;
}