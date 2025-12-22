using System.Collections.Generic;
using UnityEngine;

public class LootContainerUI : MonoBehaviour
{
    [SerializeField] private List<LootContainerUIButton> _buttons;
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;

    private SO_ItemContainer _lootContainer;

    private void Start()
    {
        Init();
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
        if (_lootContainer == null) return;

        if (_lootContainer.isChange)
        {
            Show();
            _lootContainer.isChange = false;
        }
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

    public void OnClick(int id)
    {
        _itemDragAndDrop.OnClick(_lootContainer.slots[id]);
        Show();
    }

    public void SetLootContainer(SO_ItemContainer lootContainer)
    {
        _lootContainer = lootContainer;
    }

    public void Show()
    {
        int inventorySize = _lootContainer.slots.Count;

        for (int i = 0; i < inventorySize; i++)
        {
            if (_lootContainer.slots[i].item == null)
            {
                _buttons[i].Clear();
            }
            else
            {
                _buttons[i].Set(_lootContainer.slots[i]);
            }
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
