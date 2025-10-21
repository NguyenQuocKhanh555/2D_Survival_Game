using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] private Color nightLightColor;
    [SerializeField] private Color dayLightColor;
    [SerializeField] private AnimationCurve lightColorCurve;
    [SerializeField] private Light2D globalLight;

    [SerializeField] private float _timeScale = 60f; // 1 real second = 1 in-game minute

    private float _time;
    private int _days;

    private float Hours { get { return _time / 3600f; } }
    private float Minutes { get { return _time % 3600f / 60f; } }

    private void Update()
    {
        _time += Time.deltaTime * _timeScale;
        float v = lightColorCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
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
