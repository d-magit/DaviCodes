using VRCModding.Api.Login;
using VRCModding.Api.User;
using VRCModding.Business.Services;
using VRCModding.Entities;
using Microsoft.AspNetCore.Mvc;

namespace VRCModding.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase {
    private readonly AccountInfoService accountInfoService;
    private readonly HwidService hwidService;
    private readonly IpService ipService;

    public LoginController(AccountInfoService accountInfoService, HwidService hwidService, IpService ipService) {
        this.accountInfoService = accountInfoService;
        this.hwidService = hwidService;
        this.ipService = ipService;
    }

    [HttpPost]
    public async Task<UserModel> LoginAsync([FromBody] LoginData loginData) {
	    // Todo: If successfully authed with exactly 2 methods, this should also add the Ips or Hwids that got added to DB, to the correct User. 
        Hwid? hwid = await hwidService.GetOrCreateAsync(loginData.Hwid); // If null then someone tried a funny...
        AccountInfo? accountInfo = await accountInfoService.GetAsync(loginData.LastAccountId); // If null then first login or someone tried a funny...
        Ip? ip = await ipService.GetOrCreateAsync(HttpContext.Connection.RemoteIpAddress?.ToString()); // Idk if null error or funny too...

        return new UserModel() { Name = null }; // Todo: Implement cases for the data above.
    }
}