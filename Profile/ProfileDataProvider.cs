using System.Threading;
using Network;
using Storage;
using UI;
using UniRx;
using UnityEngine;

namespace Profile
{
	public class ProfileDataProvider : IProfileDataProvider
	{
		private IStorage _storage;

		public ProfileDataProvider(IStorage storage)
		{
			_storage = storage;
		}

		public bool IsMobile
		{
			get { return _storage.IsMobilePlatform; }
		}

		public Transform GetProfileWindow()
		{
			return _storage.Get<Transform>("Canvas/ProfileWindow(Clone)").Result;
		}
		
		public GameObject CurrentOpenedWindow
		{
			get => _storage.CurrentOpenedWindow;
			set => _storage.CurrentOpenedWindow = value;
		}

		public WindowType CurrentOpenedWindowType
		{
			get => _storage.CurrentOpenedWindowType;
			set => _storage.CurrentOpenedWindowType = value;
		}

		public ReactiveCommand Logout => _storage.Logout;

		public ReactiveCommand<WindowType> OpenWindow => _storage.OpenWindow;

		public ReactiveCommand<WindowType> CloseWindow => _storage.CloseWindow;

		public ReactiveProperty<string> UserName
		{
			get { return _storage.UserData.Value.UserName;}
			set { _storage.UserData.Value.UserName.SetValueAndForceNotify(value.Value); }
		}

		public WebRequest Request
		{
			get { return _storage.Request;}
			set { _storage.Request = value; }
		}

		public string Operator
		{
			get { return _storage.UserData.Value.Operator.Value; }
			set { _storage.UserData.Value.Operator.Value = value; }
		}

		public string Avatar
		{
			get { return _storage.UserData.Value.AvatarUrl.Value; }
			set { _storage.UserData.Value.AvatarUrl.SetValueAndForceNotify(value); }
		}

		public Transform GetAvatarPanel()
		{
			return _storage.Get<Transform>("Canvas/ProfilePanel(Clone)/Avatar").Result;
		}

		public Transform GetMainMenuPanel()
		{
			return _storage.Get<Transform>("Canvas/Menu(Clone)/MainMenu").Result;
		}
	}
}