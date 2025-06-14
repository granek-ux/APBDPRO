using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Users_Roles")]
public class UserRole
{
    [Key]
    public int Id { get; set; }

    [MaxLength(20)] public string Name { get; set; } = null!;
    
    public ICollection<User> Users { get; set; } = null!;
}