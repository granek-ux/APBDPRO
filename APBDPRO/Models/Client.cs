using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Client")]
public class Client
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)] public string Adres { get; set; } = null!;
    [MaxLength(50)] public string Email { get; set; } = null!;
    public int PhoneNumber { get; set; }

    public Person? Person { get; set; }
    public Company? Company { get; set; }

}