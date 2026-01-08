using System;
using UnityEngine;
using UnityEngine.UI;

public class FishingPanel : MonoBehaviour
{
    [SerializeField] private float _maxProgress = 100f;
    [SerializeField] private float _progressDecreaseRate = 5f;
    [SerializeField] private float _progressIncreasePerTap = 15f;
    [SerializeField] private float _fishProgressIncreaseRate = 10f;

    [SerializeField] private Slider _playerProgressBar;
    [SerializeField] private Slider _fishProgressBar;

    private float _playerProgress = 0f;
    private float _fishProgress = 0f;

    public Action<bool> fishingResult;

    private void Start()
    {
        _playerProgressBar.maxValue = _maxProgress;
        _fishProgressBar.maxValue = _maxProgress;
    }

    private void OnEnable()
    {
        _playerProgress = 0f;
        _fishProgress = 0f;
        _playerProgressBar.value = 0f;
        _fishProgressBar.value = 0f;
    }

    private void Update()
    {
        _fishProgress += _fishProgressIncreaseRate * Time.deltaTime;
        _fishProgressBar.value = _fishProgress;

        if (_fishProgress >= _maxProgress)
        {
            gameObject.SetActive(false);
            fishingResult.Invoke(false);
            return;
        }

        _playerProgress -= _progressDecreaseRate * Time.deltaTime;
        _playerProgress = Mathf.Max(0f, _playerProgress);
        _playerProgressBar.value = _playerProgress;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerProgress += _progressIncreasePerTap;
            _playerProgressBar.value = _playerProgress;
        }

        if (_playerProgress >= _maxProgress)
        {
            gameObject.SetActive(false);
            fishingResult.Invoke(true);
            return;
        }
    }
}
