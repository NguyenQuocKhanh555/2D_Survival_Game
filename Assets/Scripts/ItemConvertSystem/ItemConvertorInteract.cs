using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorInteract : Interactable
{
    [SerializeField] private string _itemConvertName;
    [SerializeField] private SO_ConvertingRecipeContainer _recipeContainer;

    private GameObject _itemConvertUI;
    private Animator _animator;

    [SerializeField] private int _timer = 0;
    [SerializeField] private SO_ConvertingRecipe _currentProcessRecipe = null;

    public ItemSlot convertResult = new ItemSlot();
    public ItemSlot[] convertMaterials;

    public Action onCompleteProcess;

    private void Start()
    {
        TimeController.instance.onTimeTick += ItemConvertorProcess;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        StartProcess();
    }

    public override void Interact(Player player)
    {
        if (_itemConvertName != null)
        {
            player.GetComponent<PlayerUIInteractController>().itemConvertingUI.TryGetValue(
                _itemConvertName, out _itemConvertUI);
        }
        _itemConvertUI.SetActive(!_itemConvertUI.activeSelf);

        ItemConvertorUI convertorUI = _itemConvertUI.GetComponent<ItemConvertorUI>();
        convertorUI.convertorInteract = this;
        convertorUI.UpdateUI();
        onCompleteProcess += convertorUI.UpdateUI;
    }

    private void ItemConvertorProcess()
    {
        if (_currentProcessRecipe == null) return;
        if (convertResult.item != _currentProcessRecipe.result.item && convertResult.item != null) return;

        if (_timer > 0)
        {
            _timer -= 1;
        }
        else
        {
            CompleteProcess();
        }
    }

    private void StartProcess()
    {
        if (!CheckMaterials()) return;
        if (_timer > 0) return;
        if (_currentProcessRecipe != null) return;
        if (convertResult.item != null && !convertResult.item.isStackable) return;

        SO_ConvertingRecipe convertingRecipe = _recipeContainer.FindRecipeWithMaterials(convertMaterials);
        if (convertingRecipe == null) return;

        _currentProcessRecipe = convertingRecipe;
        _timer = convertingRecipe.timeToProcess;
        _animator.SetBool("isProcessing", true);
    }

    private void CompleteProcess()
    {
        if (_currentProcessRecipe == null) { return; }

        for (int i = 0; i < _currentProcessRecipe.materials.Count; i++)
        {
            ItemSlot slot = null;

            for (int j = 0; j < convertMaterials.Length; j++)
            {
                if (convertMaterials[j].item == _currentProcessRecipe.materials[i].item)
                {
                    slot = convertMaterials[j];
                    break;
                }
            }

            if (slot != null)
            {
                slot.quantity -= _currentProcessRecipe.materials[i].quantity;
                if (slot.quantity <= 0)
                {
                    slot.Clear();
                }
            }
        }

        if (convertResult.item == null)
        {
            convertResult.Set(_currentProcessRecipe.result.item, _currentProcessRecipe.result.quantity);
        }
        else
        {
            convertResult.quantity += _currentProcessRecipe.result.quantity;
        }

        _animator.SetBool("isProcessing", false);
        _currentProcessRecipe = null;
        onCompleteProcess?.Invoke();
    }

    private bool CheckMaterials()
    {
        for (int i = 0; i < convertMaterials.Length; i++)
        {
            if (convertMaterials[i].item != null)
            {
                return true;
            }
        }

        return false;
    }

    public void StopProcess()
    {
        if (!CheckMaterials())
        {
            _timer = 0;
            _currentProcessRecipe = null;
            _animator.SetBool("isProcessing", false);
        }
    }
}
