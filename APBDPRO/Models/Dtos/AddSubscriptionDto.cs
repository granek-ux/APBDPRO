using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class AddSubscriptionDto
{
    [Required] [Length(10, 11)] public string PeselOrKrs { get; set; } = null!;
    
    [Required][MaxLength(50)] public string SoftwareName { get; set; } = null!;
    
    [Required][MaxLength(30)] public string Name { get; set; } = null!;
    [Required] public int RenewalPeriodDurationInMonths { get; set; }
    [Required] public double Price {get; set; }
    
}
