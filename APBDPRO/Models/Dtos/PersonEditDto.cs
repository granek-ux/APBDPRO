using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class PersonEditDto : ClientEditDto
{
    [MaxLength(50)] public string? FirstName { get; set; }
    [MaxLength(50)] public string? LastName { get; set; }
}