namespace VRCModding.Api;

public enum ErrorCodes
{ 
    Unknown ,
    InsufficientCredentials,
    HwidIsRequired,
    UserIdIsRequired,
    IpIsRequired,
    FailedToFetchIp,
    FailedToDeduceUser,
    UserHoneypotted
}