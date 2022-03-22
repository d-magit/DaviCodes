namespace VRCModding.Api.User;

public class UserModel
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public string Permissions { get; set; }
    
    public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    
    public DateTime CreationDateUtc { get; set; } = DateTime.UtcNow;
}