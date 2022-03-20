using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace VRCModding.Entities;

public class Account : IUserInfo
{
	#region Base
	[Key]
	[MaxLength(40)]
	public string Id { get; set; }
    
	[ForeignKey("User")]
	public Guid UserFK { get; set; }
	[JsonIgnore]
	public User User { get; set; }
    
	public DateTime LastLogin { get; set; } = DateTime.UtcNow;
	#endregion

    [ForeignKey("CurrentDisplayName")]
    public string? DisplayNameFK { get; set; }
    [JsonIgnore]
    public DisplayName? CurrentDisplayName { get; set; }
    
    public List<UsedDisplayName> DisplayNameHistory { get; set; }
}

public class UsedDisplayName
{
	[Key]
	[ForeignKey("Account")]
	public string AccountFK { get; set; }
	[JsonIgnore]
	public Account Account { get; set; }

	[Key]
	[ForeignKey("DisplayName")]
	public string DisplayNameFK { get; set; }
	[JsonIgnore]
	public DisplayName DisplayName { get; set; }

	public DateTime FirstSeen { get; set; } = DateTime.UtcNow;
	public DateTime LastUsage { get; set; } = DateTime.UtcNow;
}

public class DisplayName
{
	[Key] 
	public string Name { get; set; }
    
	public Account? CurrentAccount { get; set; }
    
	public List<UsedDisplayName> HasBeenUsedBy { get; set; }
}
