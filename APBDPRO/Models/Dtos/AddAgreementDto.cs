using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class AddAgreementDto
{
    [Required] [Length(10, 11)] public string PeselOrKrs { get; set; } = null!;
    
    [Required][MaxLength(50)] public string SoftwareName { get; set; } = null!;
    
    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }
    
    [Required] public int HowMuchLongerAssistance { get; set; }
    
    [Required] public double Price { get; set; }
    
    

}