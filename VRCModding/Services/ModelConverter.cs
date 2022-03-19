using VRCModding.Api.User;
using VRCModding.Entities;

namespace VRCModding.Services;

public class ModelConverter {
	public ModelConverter() { }

	public UserModel ToModel(User user) =>
		new() {
			Name = user.Name,
			LastLogin = user.LastLogin,
			CreationDateUtc = user.CreationDateUtc
		};
}
