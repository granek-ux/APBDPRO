using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;


[Table("Person")]
public class Person
{
    [Key]
    [ForeignKey(nameof(Client))]
    public int Id { get; set; }
    [MaxLength(50)] public string FirstName { get; set; } = null!;
    [MaxLength(50)] public string LastName { get; set; } = null!;
    public int PESEL { get; set; }
    
    public Client Client { get; set; } = null!;
}