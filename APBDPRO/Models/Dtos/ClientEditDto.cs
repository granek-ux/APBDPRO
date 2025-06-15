using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models.Dtos;

public class ClientEditDto
{
    [MaxLength(20)] public string? Address { get; set; }
    [MaxLength(20)] public string? Email { get; set; }
    public int? PhoneNumber { get; set; }
}