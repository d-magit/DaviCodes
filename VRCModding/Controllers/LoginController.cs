using VRCModding.Api.Login;
using VRCModding.Api.User;
using VRCModding.Business;
using VRCModding.Business.Services;
using VRCModding.Entities;
using VRCModding.Services;
using Microsoft.AspNetCore.Mvc;

namespace VRCModding.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase {
	private readonly UserService userService;
	private readonly AccountService accountService;
    private readonly HwidService hwidService;
    private readonly IpService ipService;
    private readonly ModelConverter modelConverter;
    private readonly ExceptionBuilder exceptionBuilder;

    public LoginController(UserService userService, AccountService accountService, HwidService hwidService, IpService ipService, ModelConverter modelConverter, ExceptionBuilder exceptionBuilder) {
	    this.userService = userService;
	    this.accountService = accountService;
        this.hwidService = hwidService;
        this.ipService = ipService;
        this.modelConverter = modelConverter;
        this.exceptionBuilder = exceptionBuilder;
    }

    /// <summary>
    /// Tries to either create a new user (if none found) or deduce who the user is (if no ambiguity) through the received data.
    /// Will update the user if extra information is provided.
    /// </summary>
    /// <param name="loginData">Contains user HWID and ID</param>
    /// <returns>UserModel with obtained or generated user's data</returns>
    /// <exception cref="ApiException"></exception>
    [HttpPost]
    public async Task<UserModel> LoginAsync([FromBody] LoginData loginData) { // Todo: Debug
	    var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

	    // Checks if parameters fulfill minimum of two to authenticate
	    var info = new [] { loginData.Hwid, loginData.LastAccountId, ip }.Where(s => !string.IsNullOrEmpty(s));
	    var infoAsStrings = info as string?[] ?? info.ToArray();
	    if (infoAsStrings.Length < 2)
		    throw exceptionBuilder.Api(Api.ErrorCodes.InsufficientCredentials, infoAsStrings);

	    //// Mounts info array and possible GUIDs array. Detail: IUserInfos cannot contain null UserFK/User.
	    // Boxed because these need to be casted back to each type later on update
	    var userInfosBoxed = new (string? id, object? userInfo)[] {
		    (loginData.Hwid, await hwidService.GetAsync(loginData.Hwid)), 
		    (loginData.LastAccountId, await accountService.GetAsync(loginData.LastAccountId)), 
		    (ip, await ipService.GetAsync(ip))
	    };
	    
	    // Unboxing because values need to be accessed here
	    var infoUnboxed = userInfosBoxed.Select(ui => (ui.id, (IUserInfo?)ui.userInfo));
	    var infoUnboxedArr = infoUnboxed as (string? id, IUserInfo? userInfo)[] ?? infoUnboxed.ToArray();
	    
	    // Grouping by Guid to separate each case later on
	    var infoGroupedByUserFks = infoUnboxedArr.GroupBy(d => d.userInfo?.UserFK);
	    var infoGroupedByUserFksArr = infoGroupedByUserFks as IGrouping<Guid?,(string? id, IUserInfo? userInfo)>[] ?? infoGroupedByUserFks.ToArray();
	    ////
	    
	    // Filters out any null userInfos and then tries to deduce who the user is
	    var distinctUserFks = infoGroupedByUserFksArr.Where(g => g.Key != null);
	    var user = distinctUserFks.Count() switch {
		    0 => await userService.CreateAsync(userInfosBoxed), // User provided enough info but wasn't known
		    1 => infoGroupedByUserFksArr[0].Count() switch { // User was known
			    1 => throw exceptionBuilder.Api(Api.ErrorCodes.UserHoneypotted, infoUnboxedArr), // Todo: Only one piece of data was known about user, so honeypot
			    2 => await userService.UpdateAsync(infoUnboxedArr.First(u => u.userInfo?.User != null).userInfo!.User, userInfosBoxed, isLogin: true), // User provided enough and/or extra data and so we check and store that new info
			    _ => infoUnboxedArr.First(u => u.userInfo?.User != null).userInfo!.User, // User provided more than enough data and was able to log in, nothing was extra because all was known // Todo: This shall go inside the function for updating the DateTimes
		    },
		    _ => throw exceptionBuilder.Api(Api.ErrorCodes.FailedToDeduceUser, infoUnboxedArr) // Todo: Failed to deduce user. This can be improved to detect users with multiple instances in db...
	    };
		// Todo: Report user login to Discord bot
		// Todo: Add i18n, ErrorResources, Discord Notifications, Templates 
	    return modelConverter.ToModel(user);
    }
}