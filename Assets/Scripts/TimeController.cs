using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] private Color _nightLightColor;
    [SerializeField] private Color _dayLightColor;
    [SerializeField] private AnimationCurve _lightColorCurve;
    [SerializeField] private Light2D _globalLight;

    [SerializeField] private float _timeScale = 60f; // 1 real second = 1 in-game minute

    private float _time;
    private int _days;

    private float Hours { get { return _time / 3600f; } }
    private float Minutes { get { return _time % 3600f / 60f; } }

    private void Update()
    {
        _time += Time.deltaTime * _timeScale;
        float v = _lightColorCurve.Evaluate(Hours);
        Color c = Color.Lerp(_dayLightColor, _nightLightColor, v);
        _globalLight.color = c;
        if (_time > secondsInDay)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        _time = 0;
        _days++;
    }
}
