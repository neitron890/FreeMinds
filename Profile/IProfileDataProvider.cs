using Network;
using UI;
using UniRx;
using UnityEngine;

namespace Profile
{
	public interface IProfileDataProvider
	{
		bool IsMobile { get; }
		Transform GetProfileWindow();
		
		GameObject CurrentOpenedWindow { get; set; }
		
		WindowType CurrentOpenedWindowType { get; set; }

		ReactiveCommand Logout { get; }
		
		ReactiveCommand<WindowType> OpenWindow { get; }
		
		ReactiveCommand<WindowType> CloseWindow { get; }

		ReactiveProperty<string> UserName { get; set; }
		WebRequest Request { get; set; }
		string Operator { get; set; }
		string Avatar { get; set; }

		Transform GetAvatarPanel();

		Transform GetMainMenuPanel();
	}
}