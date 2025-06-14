using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class CompanyDto : ClientDto
{
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;

    [Required]
    [StringLength(11)]
    public string KRS { get; set; } = null!;
}