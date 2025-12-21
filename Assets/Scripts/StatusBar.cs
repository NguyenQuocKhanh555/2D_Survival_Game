using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider _sliderBar;

    public void SetUpBar(float currentValue, float maxValue)
    {
        _sliderBar.maxValue = maxValue;
        _sliderBar.value = currentValue;
    }
}
