using System.ComponentModel.DataAnnotations;

namespace VRCModding.Api.Login;

public class LoginData
{
    [MaxLength(40)]
    public string? Hwid { get; set; }
    
    [MaxLength(40)]
    public string? LastAccountId { get; set; }
}