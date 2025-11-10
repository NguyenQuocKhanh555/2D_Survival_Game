using UnityEngine;
using UnityEngine.UI;

public class CraftRecipeButton : MonoBehaviour
{
    [SerializeField] private Image _craftItemIcon;

    public void Show(SO_Item craftItem)
    {
        this.gameObject.SetActive(true);
        _craftItemIcon.sprite = craftItem.itemIcon;
    }
}
