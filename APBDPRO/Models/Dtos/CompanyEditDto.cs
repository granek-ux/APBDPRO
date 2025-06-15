using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class CompanyEditDto : ClientEditDto
{
    [MaxLength(50)] public string? Name { get; set; }
}