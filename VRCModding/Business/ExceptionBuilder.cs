using VRCModding.Api;
using VRCModding.Services;

namespace VRCModding.Business; 

public class ExceptionBuilder {
	private readonly ModelConverter modelConverter;

	public ExceptionBuilder(ModelConverter modelConverter) {
		this.modelConverter = modelConverter;
	} 
	
	public ApiException Api(ErrorCodes code, object? details = null) {
		var message = code switch {
			ErrorCodes.Unknown => "An Unknown error has occurred.",
			ErrorCodes.InsufficientCredentials => "At least two credentials are necessary to log in.",
			ErrorCodes.FailedToFetchIp => "An error occurred while trying to fetch User's IP, and the operation failed to complete.",
			ErrorCodes.FailedToDeduceUser => "An error occurred while trying to deduce User from given info, and the operation failed to complete.",
			ErrorCodes.UserHoneypot => "Honeypot Users cannot login.",
			_ => string.Empty
		};
		return new ApiException(new ErrorModel { Code = code, Message = message, 
			Details = modelConverter.ToModel(details)});
		//	Details = getDetailsDictionary(details)
	}

	// private static Dictionary<string, string>? getDetailsDictionary(object? details) {
	// 	if (details == null) {
	// 		return null;
	// 	}
	// 	var dic = new Dictionary<string, string>();
	// 	foreach (var descriptor in details.GetType().GetProperties()) {
	// 		dic[descriptor.Name] = descriptor.GetValue(details, null)?.ToString() ?? "null";
	// 	}
	// 	return dic;
	// }
}

public class ApiException : Exception {
	public ErrorModel Error { get; set; }

	public ApiException(ErrorModel error) : base(error.Message) {
		Error = error;
	}
}
