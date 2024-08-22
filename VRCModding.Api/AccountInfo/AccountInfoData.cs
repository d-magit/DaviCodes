using System.ComponentModel.DataAnnotations;

namespace VRCModding.Api.AccountInfo;

public class AccountInfoData
{
    [Required(ErrorMessage = "The User's ID is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "The User's current display name is required.")]
    public string CurrentDisplayName { get; set; }
}