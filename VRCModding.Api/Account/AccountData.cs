using System.ComponentModel.DataAnnotations;

namespace VRCModding.Api.Account;

public class AccountData
{
    [Required(ErrorMessage = "The User's ID is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "The User's current display name is required.")]
    public string CurrentDisplayName { get; set; }
}