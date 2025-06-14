using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class ClientDto
{
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    [Required]
    public int PhoneNumber { get; set; }
    [Required]
    [MaxLength(50)] 
    public string Address { get; set; } = null!;
}