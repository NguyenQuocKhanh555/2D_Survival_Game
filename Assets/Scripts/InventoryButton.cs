using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryButton : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQuantity;

    public void SetIcon(Sprite icon)
    {
        _itemIcon.gameObject.SetActive(true);
        _itemIcon.sprite = icon;
    }
    public void SetQuantity(string quantity)
    {
        _itemQuantity.gameObject.SetActive(true);
        _itemQuantity.text = quantity;
    }
}
