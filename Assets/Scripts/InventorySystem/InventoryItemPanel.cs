using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPanel : MonoBehaviour, IItemPanel
{
    [SerializeField] private List<InventoryItemButton> _buttons;
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetSourcePanel();
        SetIndex();
        SetLockButton();
        Show();
    }

    private void OnEnable()
    {
        Clear();
        Show();
        InventoryManager.instance.onInventoryChange += Show;
        InventoryManager.instance.onInventorySizeChange += SetLockButton;
        InventoryManager.instance.onInventorySizeChange += Show;
    }

    private void OnDisable()
    {
        InventoryManager.instance.onInventoryChange -= Show;
        InventoryManager.instance.onInventorySizeChange -= SetLockButton;
        InventoryManager.instance.onInventorySizeChange -= Show;
    }

    public void SetSourcePanel()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetItemPanel(this);
        }
    }

    public void SetIndex()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetInventoryIndex(i);
        }
    }

    public void SetLockButton()
    {
        int inventorySize = InventoryManager.instance.inventorySize;

        for (int i = 20; i < _buttons.Count; i++)
        {
            _buttons[i].UnlockButton();
        }

        for (int i = inventorySize; i < _buttons.Count; i++)
        {
            _buttons[i].LockButton();
        }
    }

    public void OnClick(int id)
    {
        _itemDragAndDrop.OnClick(InventoryManager.instance.GetItemSlotInInventory(id));
        Show();
    }

    public void Show()
    {
        int inventorySize = InventoryManager.instance.inventorySize;

        for (int i = 0; i < inventorySize; i++)
        {
            if (InventoryManager.instance.GetItemInSlot(i) == null)
            {
                _buttons[i].Clear();
            }
            else
            {
                _buttons[i].Set(InventoryManager.instance.GetItemSlotInInventory(i));
            }
        }

        for (int i = inventorySize; i < _buttons.Count; i++)
        {
            _buttons[i].SetLockIcon();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Clear();
        }
    }
}
