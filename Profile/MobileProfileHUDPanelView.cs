using Profile;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MobileProfileHUDPanelView : MonoBehaviour
{
    [SerializeField] 
    private Image _avatar;
    [SerializeField] 
    private Image _experience;
    [SerializeField] 
    private TextMeshProUGUI _level;

    public void Init(IReactiveProperty<Sprite> avatar, IReactiveProperty<ExperienceData> experienceData, ReactiveProperty<int> level)
    {
        avatar.Subscribe(value =>
        {
            _avatar.sprite = value;
        });
            
        experienceData.Subscribe(value =>
        {
            _experience.fillAmount = (value.Current/ (float)value.Total);
        });

        level.Subscribe(value =>
        {
            _level.text = level.Value.ToString();
        });
    }
}
