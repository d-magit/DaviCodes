using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VRCModding.Configuration; 

public class ValidateModelStateAttribute: ActionFilterAttribute {
	private static readonly DefaultContractResolver SharedContractResolver = new() {
		NamingStrategy = new CamelCaseNamingStrategy {
			ProcessDictionaryKeys = true
		},
	};

	private static readonly JsonSerializerSettings SerializerSettings;

	static ValidateModelStateAttribute() {
		SerializerSettings = new JsonSerializerSettings {
			ContractResolver = SharedContractResolver
		};
	}

	public override void OnActionExecuting(ActionExecutingContext context) {
		if (!context.ModelState.IsValid) {
			context.Result = new JsonResult(new SerializableError(context.ModelState), SerializerSettings) {
				StatusCode = (int)HttpStatusCode.BadRequest
			};
		}
	}}
