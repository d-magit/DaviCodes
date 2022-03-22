using VRCModding.Api.User;
using VRCModding.Entities;
using Newtonsoft.Json;

namespace VRCModding.Services;

public class ModelConverter {
	public ModelConverter() { }

	public UserModel ToModel(User user) =>
		new() {
			Id = user.Id,
			Name = user.Name,
			Permissions = user.Permissions.ToString(),
			LastLogin = user.LastLogin,
			CreationDateUtc = user.CreationDateUtc
		};

	public object? ToModel(object? @object) =>
		JsonConvert.DeserializeObject(JsonConvert.SerializeObject(@object,
			new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
}
