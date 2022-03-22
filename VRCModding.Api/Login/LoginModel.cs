using VRCModding.Api.User;

namespace VRCModding.Api.Login;

public class LoginModel
{
    public string BearerToken { get; set; }
    
    public UserModel User { get; set; }
    
    public object? ProvidedCredentials { get; set; }
}