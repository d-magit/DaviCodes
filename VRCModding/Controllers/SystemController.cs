using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace VRCModding.Controllers; 

[ApiController]
[Route("api/system")]
public class SystemController : Controller {
	public object GetSystemInfo() {
		var version = Assembly.GetEntryAssembly()?.GetName().Version;
		return new {
			version = $"{version?.Major}.{version?.Minor}.{version?.Build}"
		};
	}
}