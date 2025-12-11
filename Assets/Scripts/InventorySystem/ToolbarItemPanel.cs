using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarItemPanel : MonoBehaviour, IItemPanel
{
    [SerializeField] private List<ToolbarItemButton> _buttons;
    [SerializeField] private PlayerToolbarController _toolbarController;

    private int _currentSelectedTool;

    private void Start()
    {
        Init();
        _toolbarController.onToolbarIndexChanged += SelectedHighlight;
        InventoryManager.instance.onInventoryChange += Show;
        SelectedHighlight(0);
    }

    public void Init()
    {
        SetSourcePanel();
        SetIndex();
        Show();
    }

    public void EnableToolbar()
    {
        Clear();
        Show();
        InventoryManager.instance.onInventoryChange += Show;
    }

    private void OnDisable()
    {
        InventoryManager.instance.onInventoryChange -= Show;
    }

    public void Clear()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Clear();
        }
    }

    public void OnClick(int id)
    {
        _toolbarController.Set(id);
    }

    public void SetIndex()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetToolbarIndex(i);
        }
    }

    public void SetSourcePanel()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetItemPanel(this);
        }
    }

    public void Show()
    {
        int inventorySize = InventoryManager.instance.inventorySize;

        for (int i = 0; i < inventorySize && i < _buttons.Count; i++)
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
    }

    public void SelectedHighlight(int selectedIndex)
    {
        _buttons[_currentSelectedTool].SelectedHighlight(false);
        _currentSelectedTool = selectedIndex;
        _buttons[_currentSelectedTool].SelectedHighlight(true);
    }
}
