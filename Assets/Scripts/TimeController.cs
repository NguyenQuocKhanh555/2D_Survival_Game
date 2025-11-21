using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;
    const float phaseLength = 60f;
    const float phasesInDay = 1440f;

    [SerializeField] private Color _nightLightColor;
    [SerializeField] private Color _dayLightColor;
    [SerializeField] private AnimationCurve _lightColorCurve;
    [SerializeField] private Light2D _globalLight;

    [SerializeField] private float _timeScale = 60f; // 1 real second = 1 in-game minute

    private float _time;
    private int _days;

    private float Hours { get { return _time / 3600f; } }
    private float Minutes { get { return _time % 3600f / 60f; } }

    public static TimeController instance;
    public Action onTimeTick;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        _time += Time.deltaTime * _timeScale;
        
        DayLight();
        TimeAgents();

        if (_time > secondsInDay)
        {
            NextDay();
        }
    }

    private void DayLight()
    {
        float v = _lightColorCurve.Evaluate(Hours);
        Color c = Color.Lerp(_dayLightColor, _nightLightColor, v);
        _globalLight.color = c;
    }

    private int oldPhase = -1;
    private void TimeAgents()
    {
        if (oldPhase == -1)
        {
            oldPhase = CalculatePhase();
        }

        int currentPhase = CalculatePhase();

        while (oldPhase < currentPhase)
        {
            oldPhase += 1;
            onTimeTick?.Invoke();
        }
    }

    private int CalculatePhase()
    {
        return (int)(_time / phaseLength) + (int)(_days * phasesInDay);
    }

    private void NextDay()
    {
        _time -= secondsInDay;
        _days++;
    }

    public float GetTime()
    {
        return _time;
    }
}
