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
    [SerializeField] private int _lastOpenedTime;
    [SerializeField] private int _numOfProcessRecipe;

    public ItemSlot convertResult = new ItemSlot();
    public ItemSlot[] convertMaterials;

    public Action onCompleteProcess;

    private void Start()
    {
        //TimeController.instance.onTimeTick += ItemConvertorProcess;
        _animator = GetComponent<Animator>();
    }

/*    private void Update()
    {
        StartProcess();
    }*/

    public override void Interact(Player player)
    {
        if (_itemConvertName != "" && _itemConvertUI == null)
        {
            player.GetComponent<PlayerUIInteractController>().itemConvertingUI.TryGetValue(
                _itemConvertName, out _itemConvertUI);
        }

        _itemConvertUI.SetActive(!_itemConvertUI.activeSelf);
        ItemConvertorUI convertorUI = _itemConvertUI.GetComponent<ItemConvertorUI>();

        if (_itemConvertUI.activeSelf)
        {
            SimulateProcess();
            TimeController.instance.onTimeTick += ItemConvertorProcess;
            convertorUI.convertorInteract = this;
            convertorUI.UpdateUI();
            onCompleteProcess += convertorUI.UpdateUI;
        }
        else
        {
            TimeController.instance.onTimeTick -= ItemConvertorProcess;
            convertorUI.convertorInteract = null;
            _lastOpenedTime = TimeController.instance.GetTime();
        }

        /*ItemConvertorUI convertorUI = _itemConvertUI.GetComponent<ItemConvertorUI>();
        convertorUI.convertorInteract = this;
        convertorUI.UpdateUI();
        onCompleteProcess += convertorUI.UpdateUI;*/
    }

    private void SimulateProcess()
    {
        if (!CheckMaterials()) return;
        if (_currentProcessRecipe == null) return;

        int currentTime = TimeController.instance.GetTime();
        int timeDiff = currentTime - _lastOpenedTime;

        if (timeDiff > 0)
        {
            int resultQuantity = timeDiff / _currentProcessRecipe.timeToProcess;
            int timeRemainder = timeDiff % _currentProcessRecipe.timeToProcess;

            if (resultQuantity == _numOfProcessRecipe)
            {
                convertResult.Set(_currentProcessRecipe.result.item, _numOfProcessRecipe);
                convertMaterials[0].Set(null, 0);
                _currentProcessRecipe = null;
                _timer = 0;
                _numOfProcessRecipe = 0;
                _animator.SetBool("isProcessing", false);
            }
            else
            {
                if (convertResult.item == null)
                {
                    convertResult.Set(_currentProcessRecipe.result.item, 0);
                }
                convertResult.quantity += resultQuantity;
                convertMaterials[0].quantity -= (_currentProcessRecipe.materials[0].quantity * resultQuantity);
                _timer = _currentProcessRecipe.timeToProcess - timeRemainder;
                _numOfProcessRecipe -= resultQuantity;
            }
        }
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

    public void StartProcess()
    {
        if (!CheckMaterials()) return;
        //if (_timer > 0) return;
        //if (_currentProcessRecipe != null) return;
        if (convertResult.item != null && !convertResult.item.isStackable) return;

        SO_ConvertingRecipe convertingRecipe = _recipeContainer.FindRecipeWithMaterials(convertMaterials);
        _currentProcessRecipe = convertingRecipe;
        if (convertingRecipe == null) return;

        _numOfProcessRecipe = GetMaxResultQuantity();
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
            _numOfProcessRecipe -= 1;
        }
        else
        {
            convertResult.quantity += _currentProcessRecipe.result.quantity;
            _numOfProcessRecipe -= 1;
        }

        _currentProcessRecipe = _numOfProcessRecipe <= 0 ? null : _currentProcessRecipe;
        onCompleteProcess?.Invoke();
        if (_currentProcessRecipe == null)
            _animator.SetBool("isProcessing", false);
        else
            _timer = _currentProcessRecipe.timeToProcess;
    }

    private bool CheckMaterials()
    {
        for (int i = 0; i < convertMaterials.Length; i++)
        {
            if (convertMaterials[i].item == null)
            {
                return false;
            }
        }

        return true;
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

    private int GetMaxResultQuantity()
    {
        if (_currentProcessRecipe == null) { return 0; }

        int quantity = 0;
        foreach (ItemSlot material in _currentProcessRecipe.materials)
        {
            for (int i = 0; i < convertMaterials.Length; i++)
            {
                if (material.item == convertMaterials[i].item)
                {
                    int result = convertMaterials[i].quantity / material.quantity;
                    if (i == 0)
                    {
                        quantity += result; 
                    }
                    quantity = result < quantity ? result : quantity;
                }
            }
        }

        return quantity;
    }
}
