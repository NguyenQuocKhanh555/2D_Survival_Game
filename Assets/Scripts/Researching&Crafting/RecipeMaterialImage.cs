using UnityEngine;
using UnityEngine.UI;

public class RecipeMaterialImage : MonoBehaviour
{
    [SerializeField] private Image _iconImage;

    public void Show(Sprite itemIcon)
    {
        _iconImage.sprite = itemIcon;
    }
}
