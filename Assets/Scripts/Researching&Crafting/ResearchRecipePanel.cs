using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchRecipePanel : MonoBehaviour
{
    [SerializeField] private ResearchItemButton _researchItemButton;
    [SerializeField] private SO_CraftingRecipeContainer _recipeContainer;
    [SerializeField] RectTransform _recipeScrollViewContent;
    [SerializeField] private List<RecipePanel> _recipePanels;

    public void OnClickResearchItemButton()
    {
        List<SO_CraftingRecipe> recipes = new List<SO_CraftingRecipe>();

        for (int i = 0; i < _researchItemButton.researchedItem.slots.Count; i++)
        {
            SO_Item materialItem = _researchItemButton.researchedItem.slots[i].item;

            recipes = recipes.Union(_recipeContainer.FindRecipeWithMaterial(materialItem)).ToList();
        }

        Show(recipes, _researchItemButton.researchedItem);
    }

    private void Show(List<SO_CraftingRecipe> recipes, SO_ItemContainer researchedItems)
    {
        for (int i = 0;i < recipes.Count;i++)
        {
            _recipePanels[i].Show(recipes[i], researchedItems);
        }
    }
}
