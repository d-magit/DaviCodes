using VRCModding.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRCModding.Business; 

public class ApiException : Exception {
	public ErrorModel Error { get; set; }

	public ApiException(ErrorModel error) : base(error.Message) {
		Error = error;
	}
}
