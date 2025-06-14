using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class PersonDto :ClientDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    [Required]
    [StringLength(11)]
    public string PESEL { get; set; }
}