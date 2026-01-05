using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorUI : MonoBehaviour
{
    [SerializeField] private ItemConvertorMaterialButton[] _materialButtons;
    [SerializeField] private ItemConvertorResultButton _resultButton;
    [SerializeField] private StatusBar _processBar;

    public ItemConvertorInteract convertorInteract;

    private void OnDisable()
    {
        convertorInteract.onCompleteProcess -= UpdateUI;
    }

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

    public void UpdateProcessBar(float currentValue, float maxValue)
    {
        _processBar.SetUpBar(currentValue, maxValue);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < convertorInteract.convertMaterials.Length; i++)
        {
            _materialButtons[i].Clear();
            if (convertorInteract.convertMaterials[i].item == null) { continue; }
            _materialButtons[i].Show(convertorInteract.convertMaterials[i]);
        }

        _resultButton.Clear();
        if (convertorInteract.convertResult.item == null) { return; }
        _resultButton.Show(convertorInteract.convertResult);
    }

    public void OnClickMaterialButton(int index, ItemSlot slot)
    {
        SO_Item preMaterial = convertorInteract.convertMaterials[index].item;
        convertorInteract.convertMaterials[index].Copy(slot);
        convertorInteract.StartProcess();
        UpdateUI();
        convertorInteract.StopProcess();
    }

    public void OnClickResultButton(ItemSlot slot, bool isStartProcess)
    {
        if (isStartProcess)
        {
            convertorInteract.StartProcess();
        }
        convertorInteract.convertResult.Copy(slot);
        UpdateUI();
    }
}
