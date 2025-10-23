using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;

    public bool IsInventoryOpen
    {
        get { return _inventoryUI.activeSelf; }
    }

    public void ToggleInventory()
    {
        _inventoryUI.SetActive(!_inventoryUI.activeSelf);
    }
}
