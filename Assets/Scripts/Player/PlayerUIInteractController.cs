using System.Collections.Generic;
using UnityEngine;

public class PlayerUIInteractController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _toolbarUI;
    [SerializeField] private GameObject _smelterUI;
    [SerializeField] private GameObject _campfireUI;
    [SerializeField] private GameObject _purifierUI;

    public GameObject researchTableUI;
    public Dictionary<string, GameObject> itemConvertingUI;

    private void Start()
    {
        itemConvertingUI = new Dictionary<string, GameObject>
        {
            { "Smelter", _smelterUI },
            { "Campfire", _campfireUI },
            { "Purifier", _purifierUI }
        };
    }

    public bool IsInventoryOpen
    {
        get { return _inventoryUI.activeSelf; }
    }

    public void ToggleInventory()
    {
        _inventoryUI.SetActive(!_inventoryUI.activeSelf);
        _toolbarUI.SetActive(!_toolbarUI.activeSelf);
        if (_toolbarUI.activeSelf)
        {
            _toolbarUI.GetComponent<ToolbarItemPanel>().EnableToolbar();
        }
    }
}
