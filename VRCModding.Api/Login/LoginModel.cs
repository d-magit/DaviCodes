using VRCModding.Api.User;

namespace VRCModding.Api.Login;

public class LoginModel
{
    public UserModel User { get; set; }
    
    public object? ProvidedCredentials { get; set; }
}