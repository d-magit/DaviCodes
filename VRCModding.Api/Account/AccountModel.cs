using System.ComponentModel.DataAnnotations;
using VRCModding.Api.DisplayName;

namespace VRCModding.Api.Account;

public class AccountModel
{
    [Required]
    [MaxLength(40)]
    public string Id { get; set; }

    public DisplayNameModel CurrentDisplayName { get; set; }
}