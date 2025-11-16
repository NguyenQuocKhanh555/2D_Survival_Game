using System;
using UnityEngine;

public class ItemConvertorUI : MonoBehaviour
{
    [SerializeField] private ItemConvertorMaterialButton[] _materialButtons;
    [SerializeField] private ItemConvertorResultButton _resultButton;

    public ItemConvertorInteract itemConvertorInteract;

    private void Start()
    {
        SetIndex();
    }

    private void SetIndex()
    {
        for (int i = 0; i < _materialButtons.Length; i++)
        {
            _materialButtons[i].SetIndex(i);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < itemConvertorInteract.data.currentMaterials.Count; i++)
        {
            _materialButtons[i].Clear();
            if (itemConvertorInteract.data.currentMaterials[i].item == null) { continue; }
            _materialButtons[i].Show(itemConvertorInteract.data.currentMaterials[i]);
        }

        _resultButton.Clear();
        if (itemConvertorInteract.data.currentResult.item == null) { return; }
        _resultButton.Show(itemConvertorInteract.data.currentResult);
    }

    public void OnClickMaterialButton(int index, ItemSlot slot)
    {
        Debug.Log(itemConvertorInteract.data.currentMaterials[index]);
        itemConvertorInteract.data.currentMaterials[index].Copy(slot);
        UpdateUI();
    }
}
