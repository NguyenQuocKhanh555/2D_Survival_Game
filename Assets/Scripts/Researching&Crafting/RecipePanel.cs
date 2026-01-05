using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{
    [SerializeField] private Image _resultItemIcon;
    [SerializeField] private List<RecipeMaterialImage> _recipeMaterialImages;
    [SerializeField] private GameObject _learnButton;

    public CraftingRecipeSlot craftingRecipeSlot = new CraftingRecipeSlot();
    public ResearchRecipePanel researchRecipePanel;

    private void Start()
    {
        researchRecipePanel = GetComponentInParent<ResearchRecipePanel>();
    }

    public void Show(CraftingRecipeSlot recipe, SO_ItemContainer researchItems)
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < _recipeMaterialImages.Count; i++)
        {
            _recipeMaterialImages[i].Clear();
            _recipeMaterialImages[i].gameObject.SetActive(false);
        }

        int numOfResearchedMaterials = 0;

        for (int i = 0; i < recipe.craftingRecipe.craftMaterials.Count; i++) 
        {
            _recipeMaterialImages[i].gameObject.SetActive(true);

            if (researchItems.CheckItem(recipe.craftingRecipe.craftMaterials[i]))
            {
                _recipeMaterialImages[i].Show(recipe.craftingRecipe.craftMaterials[i].item.itemIcon);
                numOfResearchedMaterials++;
            }   
        }

        _resultItemIcon.sprite = null;
        _learnButton.SetActive(false);

        if (numOfResearchedMaterials >= recipe.craftingRecipe.craftMaterials.Count)
        {
            _resultItemIcon.sprite = recipe.craftingRecipe.resultItem.item.itemIcon;
            craftingRecipeSlot.Copy(recipe);
            _learnButton.SetActive(true);
        }
    }
}
