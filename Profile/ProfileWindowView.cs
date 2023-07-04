using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Profile
{
	public class ProfileWindowView : MonoBehaviour
	{
		[SerializeField] 
		private Image _avatar;
		[SerializeField] 
		private TextMeshProUGUI _level;
		[SerializeField] 
		private TMP_InputField _name;
		[SerializeField] 
		private TextMeshProUGUI _phone;
		[SerializeField] 
		private TextMeshProUGUI _experience;
		[SerializeField] 
		private Button _changeNameButton;
		[SerializeField] 
		private Button _logoutButton;
		[SerializeField] 
		private Button _setProfile;
		[SerializeField] 
		private Button _closeButton;
		[SerializeField] 
		private Image _progressBar;
		
		private string _prevProfileNameValue;

		public void Init(
			IReactiveProperty<int> level, 
			IReactiveProperty<ExperienceData> experience,
			IReactiveProperty<string> userName,
			IReactiveProperty<string> phone,
			IReactiveProperty<string> avatarUrl,
			IProfileController profileController)
		{
			level.Subscribe(value =>
			{
				_level.text = value.ToString();
			});
			
			userName.Subscribe(value =>
			{
				_name.text = value?.ToString();
			});
			
			avatarUrl.Subscribe(value =>
			{
				if (value != null)
					UpdateObject.RunCoroutine(LoadAvatar(value));
			});
			
			experience.Subscribe(value =>
			{
				_experience.text = value.Current.ToString() + "/" + value.Total.ToString();

				_progressBar.fillAmount = (float)value.Current / (float)value.Total;
			});

			phone.Subscribe(value =>
			{
				_phone.text = value?.ToString();
			});
			
			_changeNameButton.onClick.AddListener(() =>
			{
				if (_changeNameButton.GetComponent<Image>().sprite.name == "M")
					_changeNameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Profile/M2");
				else
				{
					_changeNameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Profile/M");
					
					profileController.AcceptCommand(new ProfileCommand
					{
						CommandType = ProfileCommandType.ChangeName,
						Data = _name.text
					});
				}
			});

			var firstEnter = true;
			_name.onValueChanged.AddListener(value =>
			{
				if (firstEnter)
				{
					firstEnter = false;
					return;
				}
				_changeNameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Profile/M2");
			});
			
			_name.onEndEdit.AddListener(value =>
			{
				if (TouchData.MouseOverUIObject.Value == _changeNameButton.gameObject)
					return;
				
				profileController.AcceptCommand(new ProfileCommand
				{
					CommandType = ProfileCommandType.ChangeName,
					Data = _name.text
				});
				
				_changeNameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Profile/M");
			});
			
			_logoutButton.onClick.AddListener(() =>
			{
				profileController.AcceptCommand(new ProfileCommand
				{
					CommandType = ProfileCommandType.Logout
				});
			});
			
			_setProfile.onClick.AddListener(() =>
			{
				profileController.AcceptCommand(new ProfileCommand
				{
					CommandType = ProfileCommandType.SetProfile
				});
			});
			
			_closeButton.onClick.AddListener(() =>
			{
				profileController.AcceptCommand(new ProfileCommand
				{
					CommandType = ProfileCommandType.Close
				});
			});
		}
		
		private IEnumerator LoadAvatar(string url) 
		{
			WWW www = new WWW(url);
			yield return www;

			var texture = www.texture;
			var image = _avatar.GetComponent<Image>();
			if (texture != null)
				image.sprite = Sprite.Create(texture,
					new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
			www.Dispose();
		}
	}
}