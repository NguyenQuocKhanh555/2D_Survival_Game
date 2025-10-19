using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] Color nightLightColor;
    [SerializeField] Color dayLightColor;
    [SerializeField] AnimationCurve lightColorCurve;
    [SerializeField] Light2D globalLight;

    [SerializeField] float timeScale = 60f; // 1 real second = 1 in-game minute

    private float time;
    private int days;

    private float Hours { get { return time / 3600f; } }
    private float Minutes { get { return time % 3600f / 60f; } }

    private void Update()
    {
        time += Time.deltaTime * timeScale;
        float v = lightColorCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        globalLight.color = c;
        if (time > secondsInDay)
        {
            NextDay();
        }
    }

    private void NextDay()
    {
        time = 0;
        days++;
    }
}
