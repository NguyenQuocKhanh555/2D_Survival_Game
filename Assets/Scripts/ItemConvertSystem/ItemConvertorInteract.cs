using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConvertorData
{
    public List<ItemSlot> currentMaterials;
    public ItemSlot currentResult;
    public int timer;
    public SO_ConvertingRecipe currentProcessRecipe;

    public ItemConvertorData()
    {
        currentMaterials = new List<ItemSlot>();
        currentResult = new ItemSlot();
    }
}

public class ItemConvertorInteract : Interactable
{
    [SerializeField] private string _itemConvertName;
    [SerializeField] private SO_ConvertingRecipeContainer _recipeContainer;
    [SerializeField] private int _numOfMaterials;

    private GameObject _itemConvertUI;
    private Animator _animator;
    
    public ItemConvertorData data;
    public Action onCompleteProcess;

    private void Start()
    {
        TimeController.instance.onTimeTick += ItemConvertorProcess;
        _animator = GetComponent<Animator>();
/*        if (data == null)
        {
            data = new ItemConvertorData();
        }*/
        for (int i = 0; i < _numOfMaterials; i++)
        {
            data.currentMaterials.Add(new ItemSlot());
        }
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
        _itemConvertUI.GetComponent<ItemConvertorUI>().itemConvertorInteract = this;
        _itemConvertUI.GetComponent<ItemConvertorUI>().UpdateUI();
    }

    private void ItemConvertorProcess()
    {
        if (data.currentProcessRecipe == null) return;
        if (data.currentResult.item != data.currentProcessRecipe.result.item) return;

        if (data.timer > 0)
        {
            data.timer -= 1;
        }
        else
        {
            CompleteProcess();
        }
    }

    private void StartProcess()
    {
        if (data.currentMaterials.Count <= 0) return;
        if (data.timer > 0) return;
        if (data.currentProcessRecipe != null) return;

        SO_ConvertingRecipe convertingRecipe = _recipeContainer.FindRecipeWithMaterials(data.currentMaterials);
        if (convertingRecipe ==  null) return;

        data.currentProcessRecipe = convertingRecipe;
        data.timer = convertingRecipe.timeToProcess;
        _animator.SetBool("isProcessing", true);
    }

    private void CompleteProcess()
    {
        for (int i = 0; i < data.currentProcessRecipe.materials.Count; i++)
        {
            ItemSlot slot = data.currentMaterials.Find(x => x.item == data.currentProcessRecipe.materials[i].item);
            if (slot != null)
            {
                slot.quantity -= data.currentProcessRecipe.materials[i].quantity;
                if (slot.quantity <= 0)
                {
                    slot.Clear();
                } 
            }
        }

        if (data.currentResult.item == null)
        {
            data.currentResult.Set(data.currentProcessRecipe.result.item, data.currentProcessRecipe.result.quantity);
        }
        else
        {
            data.currentResult.quantity += data.currentProcessRecipe.result.quantity;
        }

        _animator.SetBool("isProcessing", false);
        data.currentProcessRecipe = null;
        onCompleteProcess?.Invoke();
    }
}
