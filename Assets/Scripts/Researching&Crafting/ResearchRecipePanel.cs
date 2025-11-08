using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchRecipePanel : MonoBehaviour
{
    [SerializeField] private ResearchItemButton _researchItemButton;
    [SerializeField] RectTransform _recipeScrollViewContent;
    [SerializeField] private List<RecipePanel> _recipePanels;

    public SO_CraftingRecipeContainer recipeContainer;

    public void OnClickResearchItemButton()
    {
        List<CraftingRecipeSlot> recipes = new List<CraftingRecipeSlot>();

        for (int i = 0; i < _researchItemButton.researchedItem.slots.Count; i++)
        {
            SO_Item materialItem = _researchItemButton.researchedItem.slots[i].item;

            recipes = recipes.Union(recipeContainer.FindRecipeWithResearchMaterial(materialItem)).ToList();
        }

        Show(recipes, _researchItemButton.researchedItem);
    }

    private void Show(List<CraftingRecipeSlot> recipes, SO_ItemContainer researchedItems)
    {
        for (int i = 0;i < recipes.Count;i++)
        {
            _recipePanels[i].Show(recipes[i], researchedItems);
        }
    }
}
