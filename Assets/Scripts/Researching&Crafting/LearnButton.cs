using UnityEngine;

public class LearnButton : MonoBehaviour
{
    [SerializeField] private RecipePanel _recipePanel;
    [SerializeField] private SO_CraftingRecipeContainer _recipeContainer;

    private CraftingRecipeSlot _craftingRecipeSlot = new CraftingRecipeSlot();

    private void OnEnable()
    {
        _craftingRecipeSlot.Copy(_recipePanel.craftingRecipeSlot);
    }

    public void OnClick()
    {
        _recipeContainer.LearnCraftingRecipe(_craftingRecipeSlot.craftingRecipe);
        this.gameObject.SetActive(false);
    }
}
