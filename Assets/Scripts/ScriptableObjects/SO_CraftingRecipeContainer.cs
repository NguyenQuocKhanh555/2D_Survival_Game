using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe Container", menuName = "Crafting/Crafting Recipe Container")]
public class SO_CraftingRecipeContainer : ScriptableObject
{
    public List<SO_CraftingRecipe> craftingRecipes;

    public List<SO_CraftingRecipe> FindRecipeWithMaterial(SO_Item materialItem)
    {
        List<SO_CraftingRecipe> recipes = new List<SO_CraftingRecipe>();

        for (int i = 0; i < craftingRecipes.Count; i++)
        {
            ItemSlot materialSlot = craftingRecipes[i].craftMaterials.Find(x => x.item == materialItem);
            if (materialSlot != null)
            {
                recipes.Add(craftingRecipes[i]);
            }
        }

        return recipes;
    }
}
