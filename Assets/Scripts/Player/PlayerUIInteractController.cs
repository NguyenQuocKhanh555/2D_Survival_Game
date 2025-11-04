using UnityEngine;

public class PlayerUIInteractController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _toolbarUI;
    [SerializeField] private GameObject _equipmentUI;

    public bool IsInventoryOpen
    {
        get { return _inventoryUI.activeSelf; }
    }

    public void ToggleInventory()
    {
        _inventoryUI.SetActive(!_inventoryUI.activeSelf);
        _toolbarUI.SetActive(!_toolbarUI.activeSelf);
        _equipmentUI.SetActive(!_equipmentUI.activeSelf);
    }
}
