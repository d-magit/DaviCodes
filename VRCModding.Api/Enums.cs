namespace VRCModding.Api;

public enum ErrorCodes
{ 
    Unknown ,
    InsufficientCredentials,
    HwidIsRequired,
    UserIdIsRequired,
    IpIsRequired,
    DisplayNameIsRequired,
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
