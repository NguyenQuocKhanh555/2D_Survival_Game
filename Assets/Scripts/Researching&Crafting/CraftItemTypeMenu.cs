using UnityEngine;
using UnityEngine.UI;

public class CraftItemTypeMenu : MonoBehaviour
{
    [SerializeField] private Button _craftedItemType;
    [SerializeField] private Button _placeableItemType;
    [SerializeField] private Button _toolType;
    [SerializeField] private Button _weaponType;
    [SerializeField] private Button _armorType;

    [SerializeField] private CraftRecipeMenu _recipeMenu;
    [SerializeField] private GameObject _craftingInfosUI;

    private void Start()
    {
        _craftedItemType.onClick.AddListener(() => OnClickItemTypeButton(ItemTypes.Material));
        _placeableItemType.onClick.AddListener(() => OnClickItemTypeButton(ItemTypes.Placeable));
        _toolType.onClick.AddListener(() => OnClickItemTypeButton(ItemTypes.Tool));
        _weaponType.onClick.AddListener(() => OnClickItemTypeButton(ItemTypes.Weapon));
        _armorType.onClick.AddListener(() => OnClickItemTypeButton(ItemTypes.Armor));
    }

    private void OnClickItemTypeButton(ItemTypes itemTypes)
    {
        _recipeMenu.ShowRecipe(itemTypes);
        _craftingInfosUI.SetActive(false);
    }
}
