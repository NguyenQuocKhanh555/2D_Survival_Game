using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] Slider sliderBar;
    [SerializeField] int maxValue;
    [SerializeField] int currentValue;

    private void Start()
    {
        sliderBar.maxValue = maxValue;
        sliderBar.value = currentValue;
    }
}
