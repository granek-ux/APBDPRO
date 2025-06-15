using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPRO.Models;

[Table("Users")]
public class User
{
    [Key] public int IdUser { get; set; }

    [MaxLength(50)] public string Login { get; set; } = null!;
    
    [MaxLength(50)] public string Password { get; set; } = null!;
    
    [MaxLength(50)] public string Salt { get; set; } = null!;
    
    [MaxLength(50)] public string RefreshToken { get; set; } = null!;
    
    public DateTime? RefreshTokenExp { get; set; }
    
    [ForeignKey(nameof(UserRole))]
    public int UserRoleId { get; set; }
    
    
    public UserRole UserRole { get; set; } = null!;
}