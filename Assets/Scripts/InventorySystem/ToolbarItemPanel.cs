using System.Collections.Generic;
using UnityEngine;

public class ToolbarItemPanel : MonoBehaviour, IItemPanel
{
    [SerializeField] private SO_ItemContainer _inventoryContainer;
    [SerializeField] private List<ToolbarItemButton> _buttons;
    [SerializeField] private PlayerToolbarController _toolbarController;

    private int _currentSelectedTool;

    private void Start()
    {
        Init();
        _toolbarController.onToolbarIndexChanged += SelectedHighlight;

        SelectedHighlight(0);
    }

    public void Init()
    {
        SetSourcePanel();
        SetIndex();
        Show();
    }

    private void OnEnable()
    {
        Clear();
        Show();
    }

    private void LateUpdate()
    {
        if (_inventoryContainer == null) return;

        if (_inventoryContainer.isChange)
        {
            Show();
            _inventoryContainer.isChange = false;
        }
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
        if (_inventoryContainer == null) return;

        for (int i = 0; i < _inventoryContainer.slots.Count && i < _buttons.Count; i++)
        {
            if (_inventoryContainer.slots[i].item == null)
            {
                _buttons[i].Clear();
            }
            else
            {
                _buttons[i].Set(_inventoryContainer.slots[i]);
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
