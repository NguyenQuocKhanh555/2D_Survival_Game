using UnityEngine;

public class LearnButton : MonoBehaviour
{
    [SerializeField] private RecipePanel _recipePanel;
    [SerializeField] private SO_CraftingRecipeContainer _learnedCraftingRecipeContainer;

    private CraftingRecipeSlot _craftingRecipeSlot = new CraftingRecipeSlot();

    private void OnEnable()
    {
        _craftingRecipeSlot.Copy(_recipePanel.craftingRecipeSlot);
    }

    public void OnClick()
    {
        _recipePanel.researchRecipePanel.globalRecipeContainer.LearnCraftingRecipe(_craftingRecipeSlot.craftingRecipe);
        _learnedCraftingRecipeContainer.AddCraftingRecipe(_craftingRecipeSlot.craftingRecipe);
        this.gameObject.SetActive(false);
        _recipePanel.researchRecipePanel.OnClickResearchButton();
    }
}
