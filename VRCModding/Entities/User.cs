using System.ComponentModel.DataAnnotations;

namespace VRCModding.Entities;

public class User
{
    [Key]
    public string Name { get; set; }
    
    public List<Hwid> KnownHWIDs { get; set; }
    
    public List<Ip> KnownIPs { get; set; }

    public List<AccountInfo> Accounts { get; set; }
    
    public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    public DateTime CreationDateUtc { get; set; } = DateTime.UtcNow;
}
