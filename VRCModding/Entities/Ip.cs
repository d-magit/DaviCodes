using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRCModding.Entities;

public class Ip : IUserInfo
{
	#region Base
	[Key]
	[MaxLength(15)]
	public string? Id { get; set; }
    
	[ForeignKey("User")]
	public Guid UserFK { get; set; }
	public User User { get; set; }
    
	public DateTime LastLogin { get; set; } = DateTime.UtcNow;
	#endregion
}
