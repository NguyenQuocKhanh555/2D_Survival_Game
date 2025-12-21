using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorInteract : Interactable
{
    [SerializeField] private string _itemConvertName;
    [SerializeField] private SO_ConvertingRecipeContainer _recipeContainer;

    private GameObject _itemConvertUI;
    private ItemConvertorUI _convertorUI;
    private Animator _animator;
    private ItemConvertorManager _manager;

    [SerializeField] private int _timer = 0;
    [SerializeField] private SO_ConvertingRecipe _currentProcessRecipe = null;
    [SerializeField] private int _lastOpenedTime;
    [SerializeField] private int _numOfProcessRecipe;
    
    private int _lastScheduledTime = 0;

    public ItemSlot convertResult = new ItemSlot();
    public ItemSlot[] convertMaterials;
    public Action onCompleteProcess;

    private void Start()
    {
        _manager = GetComponentInParent<ItemConvertorManager>();
        _animator = GetComponent<Animator>();
    }

    public override void Interact(Player player)
    {
        if (_itemConvertName != "" && _itemConvertUI == null)
        {
            player.GetComponent<PlayerUIInteractController>().itemConvertingUI.TryGetValue(
                _itemConvertName, out _itemConvertUI);
        }

        _itemConvertUI.SetActive(!_itemConvertUI.activeSelf);
        
        if (_convertorUI == null)
            _convertorUI = _itemConvertUI.GetComponent<ItemConvertorUI>();

        if (_itemConvertUI.activeSelf)
        {
            SimulateProcess();
            TimeController.instance.onTimeTick += ItemConvertorProcess;
            _convertorUI.convertorInteract = this;
            _convertorUI.UpdateUI();
            _convertorUI.UpdateProcessBar(_currentProcessRecipe != null ? _currentProcessRecipe.timeToProcess - _timer : 0, 
                _currentProcessRecipe != null ? _currentProcessRecipe.timeToProcess : 1);
            onCompleteProcess += _convertorUI.UpdateUI;
        }
        else
        {
            TimeController.instance.onTimeTick -= ItemConvertorProcess;
            _convertorUI.convertorInteract = null;
            _lastOpenedTime = TimeController.instance.GetTime();
        }
    }

    private void SimulateProcess()
    {
        if (!CheckMaterials()) return;
        if (_currentProcessRecipe == null) return;

        int currentTime = TimeController.instance.GetTime();
        int timeDiff = currentTime - _lastOpenedTime;

        if (timeDiff > 0)
        {
            int resultQuantity;

            if (timeDiff >= _timer)
            {
                resultQuantity = 1 + (timeDiff - _timer) / _currentProcessRecipe.timeToProcess;
                resultQuantity = Mathf.Min(resultQuantity, _numOfProcessRecipe);
                _timer = _currentProcessRecipe.timeToProcess - (timeDiff - _timer) % _currentProcessRecipe.timeToProcess;
            }
            else
            {
                resultQuantity = 0;
                _timer -= timeDiff;
            }

            if (resultQuantity == 0) { return; }

            if (convertResult.item == null)
            {
                convertResult.Set(_currentProcessRecipe.result.item, 0);
            }
            convertResult.quantity += resultQuantity;
            DecreaseMaterial(resultQuantity);    
            _numOfProcessRecipe -= resultQuantity;

            if (_numOfProcessRecipe <= 0)
            {
                _currentProcessRecipe = null;
            }
        }
    }

    private void DecreaseMaterial(int resultQuantity)
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
                slot.quantity -= _currentProcessRecipe.materials[i].quantity * resultQuantity;
                if (slot.quantity <= 0)
                {
                    slot.Clear();
                }
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

            if (_timer <= 0)
            {
                CompleteProcess();
            }
        }

        _convertorUI.UpdateProcessBar(_currentProcessRecipe != null ? _currentProcessRecipe.timeToProcess - _timer : 0,
            _currentProcessRecipe != null ? _currentProcessRecipe.timeToProcess : 1);
    }

    public void StartProcess()
    {
        if (!CheckMaterials()) return;
        if (convertResult.item != null && !convertResult.item.isStackable) return;

        SO_ConvertingRecipe convertingRecipe = _recipeContainer.FindRecipeWithMaterials(convertMaterials);
        bool isSameRecipe = convertingRecipe == _currentProcessRecipe;
        _currentProcessRecipe = convertingRecipe;
        if (convertingRecipe == null) return;

        _numOfProcessRecipe = GetMaxResultQuantity();
        _timer = isSameRecipe ? _timer : convertingRecipe.timeToProcess;

        if (_lastScheduledTime != 0)         
        {
            _manager.UnscheduleConvertor(this, _lastScheduledTime);
        }

        int stopTime = isSameRecipe ? TimeController.instance.GetTime() + _timer + (_numOfProcessRecipe - 1) * _currentProcessRecipe.timeToProcess
            : TimeController.instance.GetTime() + _numOfProcessRecipe * _currentProcessRecipe.timeToProcess;
        _lastScheduledTime = stopTime;

        _manager.ScheduleConvertor(this, stopTime);
        _animator.SetBool("isProcessing", true);
    }

    private void CompleteProcess()
    {
        if (_currentProcessRecipe == null) { return; }

        DecreaseMaterial(1);

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

        if (_currentProcessRecipe != null)
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
            _numOfProcessRecipe = 0;
            _animator.SetBool("isProcessing", false);
        }
    }

    public void StopAnimation()
    {
        _animator.SetBool("isProcessing", false);
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
