using System.ComponentModel.DataAnnotations;
using VRCModding.Api.DisplayName;

namespace VRCModding.Api.AccountInfo;

public class AccountInfoModel
{
    [Required]
    [MaxLength(40)]
    public string Id { get; set; }

    public DisplayNameModel CurrentDisplayName { get; set; }
}