using System.Collections.Generic;
using UI;
using System.Runtime.InteropServices;
using Network;
using UnityEngine;
using UniRx;
using Utils;

namespace Profile
{
	public class ProfileService : IProfileService
	{
		[DllImport("__Internal")]
		private static extern void SendEvent (string @event, string eventCategory, string eventAction, string eventLabel, 
			int eventValue, string userId, string userAuth, string screenName, int abonent, string eventContent, string eventContext, string buttonLocation, string actionGroup,
			string productName, string productId, string touchPoint);
		
		private readonly IProfileDataProvider _profileDataProvider;

		[DllImport("__Internal")]
		private static extern void OpenNewWindow(string url);
		
		public ProfileService(IProfileDataProvider profileDataProvider)
		{
			_profileDataProvider = profileDataProvider;
			
			_profileDataProvider.OpenWindow.Subscribe(data =>
			{
				WindowType windowType = _profileDataProvider.CurrentOpenedWindowType;
				if (windowType == WindowType.Profile)
				{
					if (_profileDataProvider.CurrentOpenedWindow != null)
					{
						_profileDataProvider.CloseWindow.Execute(windowType);
						_profileDataProvider.CurrentOpenedWindow.SetActive(false);
						_profileDataProvider.CurrentOpenedWindowType = WindowType.None;
					}
				}
			});
		}
		
		public void ShowProfileWindow()
		{
			ShowOrHideWindow(WindowType.Profile);
			
			var request = GameService.CreateApiGetRequest("/profiles/current");

			_profileDataProvider.Request = new WebRequest
			{
				Request = request,
				Status = RequestStatus.ReadyToStart,
				Type = WebRequestType.BuySoftCurrency,
				Callback = webRequest =>
				{
					if (string.IsNullOrEmpty(webRequest.Error))
					{
						Debug.Log(webRequest.Response.Value);

						var jsonObject = Json.Deserialize(webRequest.Response.Value) as Dictionary<string, object>;
						var data = jsonObject["data"] as Dictionary<string, object>;
						_profileDataProvider.UserName.Value = data["name"] as string;
						_profileDataProvider.Avatar = data["avatar"] as string;
						_profileDataProvider.Operator = data["operator"] as string;
					}
					else
					{
						Debug.LogError(webRequest.Response.Value);
					}
				}
			};
			
			if (_profileDataProvider.IsMobile)
			{
				_profileDataProvider.GetAvatarPanel().gameObject.SetActive(!_profileDataProvider.GetProfileWindow().gameObject.activeSelf);
				_profileDataProvider.GetMainMenuPanel().gameObject.SetActive(!_profileDataProvider.GetProfileWindow().gameObject.activeSelf);
			}
		}

		public void Logout()
		{
			SendEvent ("vntProfile", "profil", "button_click", "vyiti", 
				0, "", "1" ,"/main", 44, null, null, "popup", "interactions", null,
				null, "web");
			
			Close();
			_profileDataProvider.Logout.Execute();
		}

		public void SetProfile()
		{
			SendEvent ("vntProfile", "profil", "button_click", "nastroit_profil", 
				0, "", "1" ,"/main", 44, null, null, "popup", "interactions", null,
				null, "web");
			
			#if UNITY_EDITOR
				Application.OpenURL("https://profile.mts.ru");
			#else
				OpenNewWindow("https://profile.mts.ru");
			#endif
		}

		public void ChangeName(string newName)
		{
			_profileDataProvider.UserName.Value = newName;
		}

		public void Close()
		{
			_profileDataProvider.GetProfileWindow().gameObject.SetActive(false);
			
			if (_profileDataProvider.IsMobile)
			{
				_profileDataProvider.GetAvatarPanel().gameObject.SetActive(true);
				_profileDataProvider.GetMainMenuPanel().gameObject.SetActive(true);
			}
		}

		private void ShowOrHideWindow(WindowType windowType)
		{
			GameObject window = null;
			switch (windowType)
			{
				case WindowType.Profile:
					window = _profileDataProvider.GetProfileWindow().gameObject;
					break;
			}

			if (window.activeSelf)
			{
				window.SetActive(false);
				_profileDataProvider.CurrentOpenedWindow = null;
				_profileDataProvider.CurrentOpenedWindowType = WindowType.None;
				_profileDataProvider.CloseWindow.Execute(windowType);
			}
			else
			{
				_profileDataProvider.OpenWindow.Execute(windowType);
				
				window.SetActive(true);
				
				_profileDataProvider.CurrentOpenedWindow = window;
				_profileDataProvider.CurrentOpenedWindowType = windowType;
			}
		}
	}
}