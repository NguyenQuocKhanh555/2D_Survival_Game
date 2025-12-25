using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    [SerializeField] private Color _unTintedColor;
    [SerializeField] private Color _tintedColor;
    [SerializeField] private float _speed = 0.5f;

    private float _f;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Tint()
    {
        StopAllCoroutines();
        _f = 0f;
        StartCoroutine(TintScreen());
    }

    public void UnTint()
    {
        StopAllCoroutines();
        _f = 0f;
        StartCoroutine(UnTintScreen());
    }

    private IEnumerator TintScreen()
    {
        while (_f < 1f)
        {
            _f += Time.deltaTime * _speed;
            _f = Mathf.Clamp(_f, 0, 1f);

            Color c = _image.color;
            c = Color.Lerp(_unTintedColor, _tintedColor, _f);
            _image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator UnTintScreen()
    {
        while (_f < 1f)
        {
            _f += Time.deltaTime * _speed;
            _f = Mathf.Clamp(_f, 0, 1f);

            Color c = _image.color;
            c = Color.Lerp(_tintedColor, _unTintedColor, _f);
            _image.color = c;

            yield return new WaitForEndOfFrame();
        }
    }
}
