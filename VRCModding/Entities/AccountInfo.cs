using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRCModding.Entities;

public class AccountInfo
{
    [Key]
    [MaxLength(40)]
    public string Id { get; set; }
    
    [ForeignKey("User")]
    public string? UserFK { get; set; }
    public User User { get; set; }

    [ForeignKey("CurrentDisplayName")]
    public string DisplayNameFK { get; set; }
    public DisplayName CurrentDisplayName { get; set; }
    
    public List<UsedDisplayName> DisplayNameHistory { get; set; }
}
