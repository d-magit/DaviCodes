using Newtonsoft.Json;

namespace VRCModding.Api;

public class ErrorModel
{
    public Dictionary<string, object[]> Errors = new();

    [JsonIgnore] 
    public (ErrorCodes errorCode, string message, object? details) Code 
    {
        set
        {
            Errors.Add(value.errorCode.ToString(), new[] {value.message, value.details!});
            LastAddedMessage = value.message;
        }
    } 

    [JsonIgnore] public string LastAddedMessage;
}