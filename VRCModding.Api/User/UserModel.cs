namespace VRCModding.Api.User;

public class UserModel
{
    public string? Name { get; set; }
    
    public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    
    public DateTime CreationDateUtc { get; set; } = DateTime.UtcNow;
}