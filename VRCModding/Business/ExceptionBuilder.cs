using VRCModding.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRCModding.Business; 

public class ExceptionBuilder {
	public ApiException Api(Enums.ErrorCodes code, object? details = null) {
		var message = code switch {
			Enums.ErrorCodes.Unknown => "An Unknown error has occurred",
			_ => string.Empty
		};
		return new ApiException(new ErrorModel() { Code = code, Message = message, Details = getDetailsDictionary(details) });
	}

	private static Dictionary<string, string>? getDetailsDictionary(object? details) {
		if (details == null) {
			return null;
		}
		var dic = new Dictionary<string, string>();
		foreach (var descriptor in details.GetType().GetProperties()) {
			dic[descriptor.Name] = descriptor.GetValue(details, null)?.ToString() ?? "null";
		}
		return dic;
	}
}
