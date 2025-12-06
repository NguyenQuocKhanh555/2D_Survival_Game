using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider _sliderBar;
    [SerializeField] private int _maxValue;
    [SerializeField] private int _currentValue;

    private void Start()
    {
        _sliderBar.maxValue = _maxValue;
        _sliderBar.value = _currentValue;
    }
}
