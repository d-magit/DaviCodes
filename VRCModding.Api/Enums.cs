namespace VRCModding.Api;

public enum ErrorCodes
{ 
    Unknown ,
    InsufficientCredentials,
    FailedToFetchIp,
    FailedToDeduceUser,
    UserHoneypot
}

[Flags]
public enum Permissions
{
    None,
    User,
    Admin
}
