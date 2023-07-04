using TMPro;
using UniRx;
using UnityEngine;

public class HUDCurrencyView : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _hardCurrency;
    [SerializeField] 
    private TextMeshProUGUI _softCurrency;

    public void Init(IReactiveProperty<int> hardCurrency, IReactiveProperty<int> softCurrency)
    {
        hardCurrency.Subscribe(value =>
        {
            _hardCurrency.text = hardCurrency.Value.ToString();
        });
            
        softCurrency.Subscribe(value =>
        {
            _softCurrency.text = softCurrency.Value.ToString();
        });
        
    }
}
