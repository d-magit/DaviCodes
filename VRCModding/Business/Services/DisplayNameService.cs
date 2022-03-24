using VRCModding.Business.Repositories;
using VRCModding.Entities;

namespace VRCModding.Business.Services;

public class DisplayNameService {
	private readonly DisplayNameRepository displayNameRepository;

	public DisplayNameService(DisplayNameRepository displayNameRepository) {
		this.displayNameRepository = displayNameRepository;
	}

	public async Task<DisplayName> ReserveAsync(string displayName) {
		var displayNameEntity = await displayNameRepository.TryGetAsync(displayName);
		if (displayNameEntity == null)
			displayNameEntity = await displayNameRepository.CreateAsync(displayName);
		else if (displayNameEntity.CurrentAccount != null)
			displayNameEntity.CurrentAccount.DisplayNameFK = null;
		return displayNameEntity;
	}
}
