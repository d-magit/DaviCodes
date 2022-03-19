using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRCModding.Entities;

public class Hwid : IUserInfo
{
	#region Base
	[Key]
	[MaxLength(40)]
	public string? Id { get; set; }
    
	[ForeignKey("User")]
	public Guid UserFK { get; set; }
	public User User { get; set; }
    
	public DateTime LastLogin { get; set; } = DateTime.UtcNow;
	#endregion
}
