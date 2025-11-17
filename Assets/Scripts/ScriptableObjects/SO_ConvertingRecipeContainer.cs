using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Converting Recipe Container", menuName = "Converting Recipe Container")]
public class SO_ConvertingRecipeContainer : ScriptableObject
{
    public List<SO_ConvertingRecipe> recipes;

    public SO_ConvertingRecipe FindRecipeWithMaterials(ItemSlot[] itemSlots)
    {
        foreach (SO_ConvertingRecipe recipe in recipes)
        {
            bool match = true;

            foreach (ItemSlot material in recipe.materials)
            {
                ItemSlot slot = null;

                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (material.item == itemSlots[i].item)
                    {
                        slot = material;
                        break;
                    }
                }

                if (slot == null || slot.quantity < material.quantity)
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return recipe;
            }
        }

        return null;
    }
}
