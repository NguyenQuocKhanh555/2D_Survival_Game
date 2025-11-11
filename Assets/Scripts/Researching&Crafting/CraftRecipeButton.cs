using UnityEngine;
using UnityEngine.UI;

public class CraftRecipeButton : MonoBehaviour
{
    [SerializeField] private Image _craftItemIcon;
    [SerializeField] private Button _craftRecipeButton;

    private SO_CraftingRecipe _recipe;
    private CraftRecipeMenu _craftRecipeMenu;
    
    private void Start()
    {
        _craftRecipeMenu = GetComponentInParent<CraftRecipeMenu>();
        _craftRecipeButton.onClick.AddListener(OnClickCraftRecipeButton);
    }

    public void Show(SO_CraftingRecipe craftingRecipe)
    {
        this.gameObject.SetActive(true);
        _craftItemIcon.sprite = craftingRecipe.resultItem.item.itemIcon;
        _recipe = craftingRecipe;
    }

    private void OnClickCraftRecipeButton()
    {
        _craftRecipeMenu.craftingPanel.ShowCraftItemInfo(_recipe);
    }
}
