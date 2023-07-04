using System.Runtime.InteropServices;
using Shop;
using Storage;
using UnityEngine;

namespace Profile
{
	public class ProfileInstaller
	{
		public void Install(LocalStorage storage, IShopController shopController)
		{
			var logoutDataProvider = new LogoutDataProvider(storage);
			var logoutService = new LogoutService(logoutDataProvider);
			
			var profileDataProvider = new ProfileDataProvider(storage);
			var profileService = new ProfileService(profileDataProvider);
			var profileController = new ProfileController(profileService, shopController, logoutService);
			
			Transform canvasTransform = GameObject.Find("Root/Canvas").transform;
			
			var prefabProfileWindow = Resources.Load<GameObject>("Prefabs/" + (storage.IsMobilePlatform? "Mobile/" : "") + "ProfileWindow");
			var gameObjectProfileWindow = Object.Instantiate(prefabProfileWindow, canvasTransform);
			gameObjectProfileWindow.SetActive(false);
			gameObjectProfileWindow.GetComponent<ProfileWindowView>()
				.Init(
					storage.UserData.Value.Level,
					storage.UserData.Value.ExperienceData,
					storage.UserData.Value.UserName,
					storage.UserData.Value.Phone,
					storage.UserData.Value.AvatarUrl,
					profileController);

			var peopleCountDataProvider = new PeoplePanelDataProvider(storage);
			var peoplePanelService = new PeoplePanelService(peopleCountDataProvider);
			var peoplePanelController = new PeoplePanelController(peoplePanelService);
			
			GameObject prefabProfilePanel = null;

			if (storage.IsMobilePlatform)
			{
				prefabProfilePanel = Resources.Load<GameObject>("Prefabs/Mobile/ProfilePanel");
				prefabProfilePanel = Object.Instantiate(prefabProfilePanel, canvasTransform);
			}
			else
			{
				prefabProfilePanel = Resources.Load<GameObject>("Prefabs/ProfilePanel");
				prefabProfilePanel = Object.Instantiate(prefabProfilePanel, canvasTransform);
			}			
			
			var notEnoughHousesLabelPrefab = Resources.Load<GameObject>("Prefabs/NotEnoughHousesLabel");
			var notEnoughHousesLabel = Object.Instantiate(notEnoughHousesLabelPrefab, prefabProfilePanel.transform);
			notEnoughHousesLabel.gameObject.SetActive(false);
			
			prefabProfilePanel.GetComponent<ProfilePanelView>().Init(
				peoplePanelController,
				storage.UserData.Value.PeopleCount,
				storage.UserData.Value.PopulationLimit,
				storage.UserData.Value.CountSoftCurrency,
				storage.UserData.Value.CountHardCurrency,
				storage.UserData.Value.Level,
				storage.UserData.Value.ExperienceData,
				storage.UserData.Value.UserName,
				storage.UserData.Value.AvatarUrl,
				profileController);
		}
	}
}