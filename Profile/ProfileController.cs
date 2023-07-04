using Shop;

namespace Profile
{
	public class ProfileController : IProfileController
	{
		private readonly IProfileService _profileService;
		private readonly IShopController _shopController;
		private LogoutService _logoutService;
		
		public ProfileController(IProfileService profileService, IShopController shopController, 
			LogoutService logoutService)
		{
			_profileService = profileService;
			_shopController = shopController;
			_logoutService = logoutService;
		}
		
		public void AcceptCommand(ProfileCommand command)
		{
			switch (command.CommandType)
			{
				case ProfileCommandType.ShowProfileWindow:
					_profileService.ShowProfileWindow();
					break;
				case ProfileCommandType.Close:
					_profileService.Close();
					break;
				case ProfileCommandType.Logout:
					_logoutService.Logout();
					_profileService.Logout();
					break;
				case ProfileCommandType.SetProfile:
					_profileService.SetProfile();
					break;
				case ProfileCommandType.ShowHardShop:
					_shopController.AcceptCommand(new ShopCommand(){CommandType = ShopCommandType.ShowHard});
					break;
				case ProfileCommandType.ShowSoftShop:
					_shopController.AcceptCommand(new ShopCommand(){CommandType = ShopCommandType.ShowSoft});
					break;
				case ProfileCommandType.ChangeName:
					_profileService.ChangeName((string)command.Data);
					break;
			}
		}
	}
}