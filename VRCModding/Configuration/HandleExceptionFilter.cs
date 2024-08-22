using System.Net;
using System.Security.Cryptography;
using VRCModding.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace VRCModding.Configuration; 

public class HandleExceptionFilter: IExceptionFilter {
	private readonly ILogger<HandleExceptionFilter> logger;

	public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger) {
		this.logger = logger;
	}

	public void OnException(ExceptionContext context) {
		if (context.Exception is ApiException apiException) {
			logger.LogWarning(apiException, "An API exception was caught"); 

			context.Result = new JsonResult(apiException.Error, JsonConvert.DefaultSettings) {
				StatusCode = (int)HttpStatusCode.UnprocessableEntity
			}; 

			context.ExceptionHandled = true;
		} else {
			var rawCode = new byte[3];

			using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(rawCode); }

			var code = BitConverter.ToString(rawCode).Replace("-", "");

			logger.LogError(context.Exception, "Unhandled exception! -- Code: {ExceptionCode}", code);

			var error = new { Message = $"An unexpected error has occurred. Please contact the support and provide the following code: {code}", ExceptionCode = code };

			context.Result = new JsonResult(error, JsonConvert.DefaultSettings) {
				StatusCode = (int)HttpStatusCode.InternalServerError
			};

			context.ExceptionHandled = true;
		}
	}
}
