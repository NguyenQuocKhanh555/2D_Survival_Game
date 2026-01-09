using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemSlotsPanel : MonoBehaviour
{
    [SerializeField] private SO_PlayerBody _playerBody;
    [SerializeField] private SO_ItemContainer _equipmentContainer;
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;
    [SerializeField] private PlayerPassiveController _playerPassiveController;

    [SerializeField] private EquipmentItemButton[] _equipmentItemButtons;
    [SerializeField] private Sprite[] _holderSprites;

    public Action onChangeEquipment;
    public bool isArmorChanged = false;
    public bool isBackpackChanged = false;
    public SO_ItemSet currentItemSet;

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
        //InventoryManager.instance.onInventorySizeChange += UpdateInventorySize;
    }

    /*private void OnDisable()
    {
        InventoryManager.instance.onInventorySizeChange -= UpdateInventorySize;
    }*/

/*    private void LateUpdate()
    {
        if (_equipmentContainer == null) return;

        if (isArmorChanged)
        {
            //Show();
            UpdatePlayerParts();
            isArmorChanged = false;
        }


        if (isBackpackChanged)
        {
            //Show();
            UpdateInventorySize();
            isBackpackChanged = false;
        }
    }*/

    public void UpdateInventorySize()
    {
        int newSize = _equipmentContainer.slots[3].item != null
            ? _equipmentContainer.slots[3].item.armorData.inventorySlotsAdded
            : 20;

        if (newSize < InventoryManager.instance.inventorySize)
        {
            InventoryManager.instance.PushItemOutOfLockedSlots(newSize);
        }

        InventoryManager.instance.inventorySize = newSize;
        InventoryManager.instance.onInventorySizeChange?.Invoke();
    }

    public void UpdatePlayerParts()
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

        bool hasFullEquiped = true;
        bool hasFullSet = false;

        for (int i = 0; i < _playerBody.playerBodyParts.Length; i++)
        {
            if (_equipmentContainer.slots[i].item == null)
            {
                hasFullEquiped = false;
                break;
            }
            if (_equipmentContainer.slots[i].item.armorData.itemSet == null)
            {
                hasFullEquiped = false;
                break;
            }
        }

        if (hasFullEquiped)
        {
            hasFullSet = _equipmentContainer.slots[0].item.armorData.itemSet == _equipmentContainer.slots[1].item.armorData.itemSet
                            && _equipmentContainer.slots[1].item.armorData.itemSet == _equipmentContainer.slots[2].item.armorData.itemSet;
            if (hasFullSet)
            {
                if (currentItemSet != _equipmentContainer.slots[0].item.armorData.itemSet)
                {
                    if (currentItemSet != null)
                    {
                        _playerPassiveController.RemovePassiveEffect(currentItemSet.passiveEffects.effectType);
                    }
                    currentItemSet = _equipmentContainer.slots[0].item.armorData.itemSet;
                    _playerPassiveController.AddPassiveEffect(currentItemSet.passiveEffects.effectType);
                }
            }
            else
            {
                if (currentItemSet != null)
                {
                    _playerPassiveController.RemovePassiveEffect(currentItemSet.passiveEffects.effectType);
                    currentItemSet = null;
                }
            }
        }
        else
        {
            if (currentItemSet != null)
            {
                _playerPassiveController.RemovePassiveEffect(currentItemSet.passiveEffects.effectType);
                currentItemSet = null;
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
        //_equipmentContainer.isChange = true;
        Show();
    }

    public void Show()
    {
        if (_equipmentContainer == null) return;

        for (int i = 0; i < _equipmentContainer.slots.Count; i++)
        {
            if (_equipmentContainer.slots[i].item == null)
            {
                _equipmentItemButtons[i].Set(_holderSprites[i]);
            }
            else
            {
                _equipmentItemButtons[i].Set(_equipmentContainer.slots[i].item.itemIcon);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _equipmentItemButtons.Length; i++)
        {
            _equipmentItemButtons[i].Set(_holderSprites[i]);
        }
    }
}
