using System;
using System.Collections;
using System.Runtime.InteropServices;
using Shop;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Profile
{
    public class ProfilePanelView : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void SendEvent (string @event, string eventCategory, string eventAction, string eventLabel, 
            int eventValue, string userId, string userAuth, string screenName, int abonent, string eventContent, string eventContext, string buttonLocation, string actionGroup,
            string productName, string productId, string touchPoint);
        
        [SerializeField] private TextMeshProUGUI _labelSoftCurrency;

        [SerializeField] private TextMeshProUGUI _labelHardCurrency;

        [SerializeField] private TextMeshProUGUI _labelLevel;
        
        [SerializeField] private TextMeshProUGUI _labelName;

        [SerializeField] private Button _buttonSoftCurrency;

        [SerializeField] private Button _buttonHardCurrency;

        [SerializeField] private Button _buttonAvatar;
        
        [SerializeField] private GameObject _populationPanel;
        
        [SerializeField] private TextMeshProUGUI _population;

        [SerializeField] private TextMeshProUGUI _populationLimit;

        [SerializeField] private Image _progress;
        
        [SerializeField] private Image _avatar;

        public void Init(
            PeoplePanelController peoplePanelController, 
            IReactiveProperty<float> population, 
            IReactiveProperty<int> populationLimit,
            IReactiveProperty<int> countSoftCurrency, 
            IReactiveProperty<int> countHardCurrency,
            IReactiveProperty<int> level, 
            IReactiveProperty<ExperienceData> experience,
            IReactiveProperty<string> userName, 
            IReactiveProperty<string> avatarUrl,
            IProfileController profileController)
        {
            countSoftCurrency.Subscribe(value => { _labelSoftCurrency.SetText(value.ToString()); });

            countHardCurrency.Subscribe(value => { _labelHardCurrency.SetText(value.ToString()); });

            level.Subscribe(value => { _labelLevel.SetText(value.ToString()); });

            experience.Subscribe(value =>
            {
                _progress.fillAmount = (float) experience.Value.Current / experience.Value.Total;
            });
            
            userName.Subscribe(value =>
            {
                if (_labelName)
                    _labelName.SetText(value);
            });

            avatarUrl.Subscribe(value =>
            {
                if (value != null)
                    StartCoroutine(LoadAvatar(value));
            });

            _buttonSoftCurrency.onClick.AddListener(() =>
            {
                profileController.AcceptCommand(new ProfileCommand
                {
                    CommandType = ProfileCommandType.ShowSoftShop
                });
            });

            _buttonHardCurrency.onClick.AddListener(() =>
            {
                profileController.AcceptCommand(new ProfileCommand
                {
                    CommandType = ProfileCommandType.ShowHardShop
                });
            });

            _buttonAvatar.onClick.AddListener(() =>
            {
                SendEvent ("vntMain", "main", "element_click", "akkaunt", 
                    0, "", "1" ,"/main", 44, null, null, null, "interactions", null,
                    null, "web");
                    
                profileController.AcceptCommand(new ProfileCommand()
                {
                    CommandType = ProfileCommandType.ShowProfileWindow
                });
            });

            TouchData.MouseOverUIObject.Subscribe(value =>
            {
                peoplePanelController.MouseOver(value == _populationPanel);
            });

            population.Subscribe(value =>
            {
                _population.text = ((int)value).ToString();
            });
            
            populationLimit.Subscribe(value =>
            {
                _populationLimit.text = "/" + value.ToString();
            });
        }
        
        private IEnumerator LoadAvatar(string url) 
        {
            WWW www = new WWW(url);
            yield return www;

            var texture = www.texture;
            var image = _avatar.GetComponent<Image>();
            image.sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
            www.Dispose();
        }
    }
}