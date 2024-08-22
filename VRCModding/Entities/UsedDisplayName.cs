using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRCModding.Entities;

public class UsedDisplayName
{
    [Key]
    [ForeignKey("AccountInfo")]
    public string AccountInfoFK { get; set; }
    public AccountInfo AccountInfo { get; set; }

    [Key]
    [ForeignKey("DisplayName")]
    public string DisplayNameFK { get; set; }
    public DisplayName DisplayName { get; set; }

    public DateTime FirstSeen { get; set; } = DateTime.UtcNow;
    public DateTime LastUsage { get; set; } = DateTime.UtcNow;
}
