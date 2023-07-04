using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Profile
{
    public class PeopleCountView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _peopleCount;
        [SerializeField] 
        private TextMeshProUGUI _peopleLimitCount;

        public void Init(PeoplePanelController peoplePanelController, IReactiveProperty<float> peopleCount, IReactiveProperty<int> peopleLimit)
        {
            peopleCount.Subscribe(value =>
            {
                _peopleCount.text = peopleCount.Value.ToString();
            });
            
            peopleLimit.Subscribe(value =>
            {
                _peopleLimitCount.text = peopleLimit.Value.ToString();
            });

            TouchData.MouseOverUIObject.Subscribe(value =>
            {
                peoplePanelController.MouseOver(value == gameObject);
            });
        }
    }
}