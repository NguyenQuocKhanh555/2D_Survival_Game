using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{
    [SerializeField] private Image _resultItemIcon;
    [SerializeField] private List<RecipeMaterialImage> _recipeMaterialImages;
    [SerializeField] private GameObject _learnButton;

    public void Show(SO_CraftingRecipe recipe, SO_ItemContainer researchItems)
    {
        int numOfResearchedMaterials = 0;
        _resultItemIcon.gameObject.SetActive(true);

        for (int i = 0; i < recipe.craftMaterials.Count; i++) 
        {         
            _recipeMaterialImages[i].gameObject.SetActive(true);
            if (researchItems.CheckItem(recipe.craftMaterials[i]))
            {
                _recipeMaterialImages[i].Show(recipe.craftMaterials[i].item.itemIcon);
                numOfResearchedMaterials++;
            }   
        }

        if (numOfResearchedMaterials >= recipe.craftMaterials.Count)
        {
            _resultItemIcon.sprite = recipe.resultItem.item.itemIcon;
            _learnButton.SetActive(true);
        }
    }
}
