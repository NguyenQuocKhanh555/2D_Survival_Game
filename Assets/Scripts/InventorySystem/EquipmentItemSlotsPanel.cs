using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemSlotsPanel : MonoBehaviour
{
    [SerializeField] private SO_PlayerBody _playerBody;
    [SerializeField] private SO_ItemContainer _equipmentContainer;
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;

    [SerializeField] private EquipmentItemButton[] _equipmentItemButtons;

    public Action onChangeEquipment;

    private void Start()
    {
        Init();
    }

    private void Init()
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
        if (_equipmentContainer == null) return;

        if (_equipmentContainer.isChange)
        {
            Show();
            UpdatePlayerParts();
            _equipmentContainer.isChange = false;
        }
    }

    private void UpdatePlayerParts()
    {
        for (int i = 0; i < _playerBody.playerBodyParts.Length; i++)
        {
            if (_equipmentContainer.slots[i].item == null)
            {
                _playerBody.playerBodyParts[i].playerPart = null;
            }
            else
            {
                _playerBody.playerBodyParts[i].playerPart = _equipmentContainer.slots[i].item.armorData;
            }
        }
        onChangeEquipment?.Invoke();
    }

    public void SetSourcePanel()
    {
        for (int i = 0; i < _equipmentItemButtons.Length; i++)
        {
            _equipmentItemButtons[i].SetItemPanel(this);
        }
    }

    public void SetIndex()
    {
        for (int i = 0; i < _equipmentItemButtons.Length; i++)
        {
            _equipmentItemButtons[i].SetEquipmentSlotIndex(i);
        }
    }

    public void OnClick(int id)
    {
        _itemDragAndDrop.OnClickOnEquipmentPanel(_equipmentContainer.slots[id], id);
        _equipmentContainer.isChange = true;
        Show();
    }

    public void Show()
    {
        if (_equipmentContainer == null) return;

        for (int i = 0; i < _equipmentContainer.slots.Count; i++)
        {
            if (_equipmentContainer.slots[i].item == null)
            {
                _equipmentItemButtons[i].Clear();
            }
            else
            {
                _equipmentItemButtons[i].Set(_equipmentContainer.slots[i]);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _equipmentItemButtons.Length; i++)
        {
            _equipmentItemButtons[i].Clear();
        }
    }
}
