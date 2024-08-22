using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCModding.Api;

public class ErrorModel
{
    [JsonProperty("code")] 
    public string CodeStr { get; set; }

    [JsonIgnore] 
    public Enums.ErrorCodes Code 
    { 
        get 
        { 
            Enums.ErrorCodes res; 
            try 
            { 
                res = (Enums.ErrorCodes)Enum.Parse(typeof(Enums.ErrorCodes), CodeStr); 
            } 
            catch 
            { 
                res = Enums.ErrorCodes.Unknown; 
            } 
            return res; 
        } 
        set => CodeStr = value.ToString();
    }

    public string Message { get; set; }

    public Dictionary<string, string>? Details { get; set; }

    public override string ToString() 
    { 
        var detailsString = string.Empty; 
        if (Details is {Count: > 0})
            detailsString = Environment.NewLine + string.Join(Environment.NewLine, Details); 
        return $"{CodeStr} - {Message}{detailsString}"; 
    }
}