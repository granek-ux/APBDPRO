using System.ComponentModel.DataAnnotations;

namespace APBDPRO.Models;

public class LoginDto
{
    [Required]
    [Length(3,50)]
    public string Login { get; set; }
    [Required]
    [Length(6,50)]
    public string Password { get; set; }
}