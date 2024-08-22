using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRCModding.Entities;

public class DisplayName
{
    [Key] 
    public string Name { get; set; }
    
    public AccountInfo? CurrentAccount { get; set; }
    
    public List<UsedDisplayName> HasBeenUsedBy { get; set; }
}