using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] private Image _craftItemIcon;
    [SerializeField] private TMP_Text _craftItemDescription;
    [SerializeField] private CraftMaterialsPanel _craftMaterialsPanel;
    [SerializeField] private Button _plusCraftQuantityButton;
    [SerializeField] private Button _minusCraftQuantityButton;
    [SerializeField] private TMP_InputField _craftQuantityInputField;
    [SerializeField] private Button _craftButton;

    private SO_CraftingRecipe _currentRecipe;
    private int _craftQuantity = 1;
    private int _minCraftQuantity = 1;
    private int _maxCraftQuantity = 100;

    public SO_ItemContainer playerInventory;

    private void Start()
    {
        _plusCraftQuantityButton.onClick.AddListener(() => OnClickChangeCraftQuantityButton(true));
        _minusCraftQuantityButton.onClick.AddListener(() => OnClickChangeCraftQuantityButton(false));
        _craftQuantityInputField.onEndEdit.AddListener(OnInputEnd);
        _craftButton.onClick.AddListener(OnClickCraftButton);
    }

    public void ShowCraftItemInfo(SO_CraftingRecipe craftingRecipe)
    {
        _currentRecipe = craftingRecipe;
        UpdateCraftItemInfo();

        _craftQuantity = _minCraftQuantity;
        UpdateInputField();
    }

    private void UpdateCraftItemInfo()
    {
        this.gameObject.SetActive(true);
        _craftItemIcon.sprite = _currentRecipe.resultItem.item.itemIcon;
        _craftItemDescription.text = _currentRecipe.resultItem.item.itemName
            + "\n" + _currentRecipe.resultItem.item.itemDescription;
        _craftMaterialsPanel.ShowCraftMaterialsInfos(_currentRecipe.craftMaterials, _craftQuantity);
    }

    private void OnClickChangeCraftQuantityButton(bool isIncrease)
    {
        int amount = isIncrease ? 1 : -1;
        _craftQuantity += amount;
        _craftQuantity = Mathf.Clamp(_craftQuantity, _minCraftQuantity, _maxCraftQuantity);
        UpdateInputField();
    }

    private void OnInputEnd(string text)
    {
        if (int.TryParse(text, out int value))
        {
            _craftQuantity = Mathf.Clamp(value, _minCraftQuantity, _maxCraftQuantity);
        }
        else
        {
            _craftQuantity = _minCraftQuantity;
        }

        UpdateInputField();
    }

    private void UpdateInputField()
    {
        _craftQuantityInputField.text = _craftQuantity.ToString();
        UpdateCraftItemInfo();
    }

    private void OnClickCraftButton()
    {
        if (_currentRecipe == null) return;
        bool canAddCraftItemToInventory = _currentRecipe.resultItem.item.isStackable ?
            playerInventory.CheckFreeSpaceForStackableItem(_currentRecipe.resultItem.item, _craftQuantity) :
            playerInventory.CheckFreeSpaceForNonStackableItem(_craftQuantity);

        if (!canAddCraftItemToInventory) return;
        bool isEnoughMaterials = true;

        for (int i = 0; i < _currentRecipe.craftMaterials.Count; i++)
        {
            ItemSlot itemSlot = _currentRecipe.craftMaterials[i];
            int materialQuantity = itemSlot.quantity * _craftQuantity;
            if (materialQuantity > playerInventory.GetItemQuantity(itemSlot.item))
            {
                isEnoughMaterials = false;
                break;
            }
        }

        if (!isEnoughMaterials) return;
        playerInventory.AddItem(_currentRecipe.resultItem.item, _craftQuantity);

        for (int i = 0; i < _currentRecipe.craftMaterials.Count; i++)
        {
            ItemSlot itemSlot = _currentRecipe.craftMaterials[i];
            playerInventory.RemoveItem(itemSlot.item, itemSlot.quantity * _craftQuantity);
        }

        UpdateCraftItemInfo();
        _craftQuantity = _minCraftQuantity;
        UpdateInputField();
    }
}
