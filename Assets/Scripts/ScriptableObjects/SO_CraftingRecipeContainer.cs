using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CraftingRecipeSlot
{
    public SO_CraftingRecipe craftingRecipe;
    public bool isLearn;

    public void Copy(CraftingRecipeSlot other)
    {
        craftingRecipe = other.craftingRecipe;
        isLearn = other.isLearn;
    }

    public void Set(SO_CraftingRecipe craftingRecipe, bool isLearn)
    {
        this.craftingRecipe = craftingRecipe;
        this.isLearn = isLearn;
    }

    public void Clear()
    {
        craftingRecipe = null;
        isLearn = false;
    }
}

[CreateAssetMenu(fileName = "New Crafting Recipe Container", menuName = "Crafting/Crafting Recipe Container")]
public class SO_CraftingRecipeContainer : ScriptableObject
{
    public List<CraftingRecipeSlot> craftingRecipes;

    public List<CraftingRecipeSlot> FindRecipeCanBeLearned(SO_ItemContainer researchedItems)
    {
        List<CraftingRecipeSlot> recipes = new List<CraftingRecipeSlot>();

        for (int i = 0; i < craftingRecipes.Count; i++)
        {
            if (craftingRecipes[i].isLearn) continue;
            
            bool canBeLearned = true;
            
            for (int j = 0; j < craftingRecipes[i].craftingRecipe.craftMaterials.Count; j++)
            {
                ItemSlot materialSlot = craftingRecipes[i].craftingRecipe.craftMaterials[j];
                if (!researchedItems.CheckItem(materialSlot))
                {
                    canBeLearned = false;
                    break;
                }
            }
            
            if (canBeLearned)
            {
                recipes.Add(craftingRecipes[i]);
            }
        }

        return recipes;
    }

    public List<CraftingRecipeSlot> FindRecipeCanNotBeLearned(SO_Item materialItem, List<CraftingRecipeSlot> canBeLearnedList)
    {
        List<CraftingRecipeSlot> recipes = new List<CraftingRecipeSlot>();

        for (int i = 0; i < craftingRecipes.Count; i++)
        {
            if (craftingRecipes[i].isLearn) continue;
            if (canBeLearnedList.Contains(craftingRecipes[i])) continue;

            ItemSlot materialSlot = craftingRecipes[i].craftingRecipe.craftMaterials.Find(x => x.item == materialItem);
            if (materialSlot != null)
            {
                recipes.Add(craftingRecipes[i]);
            }
        }

        return recipes;
    }

    public void LearnCraftingRecipe(SO_CraftingRecipe learnCraftingRecipe)
    {
        CraftingRecipeSlot recipeSlot = craftingRecipes.Find(x => x.craftingRecipe == learnCraftingRecipe);
        if (recipeSlot != null)
        {
            recipeSlot.isLearn = true;
        }
    }

    public void AddCraftingRecipe(SO_CraftingRecipe recipeToAdd)
    {
        CraftingRecipeSlot emptyCraftingRecipeSlot = craftingRecipes.Find(x => x.craftingRecipe == null);
        if (emptyCraftingRecipeSlot == null) return;
        emptyCraftingRecipeSlot.Set(recipeToAdd, true);
    }

    public List<SO_CraftingRecipe> GetAllRecipesWithItemType(ItemTypes itemTypes)
    {
        List<SO_CraftingRecipe> recipes = new List<SO_CraftingRecipe>();
        
        for (int i = 0; i < craftingRecipes.Count; i++) 
        {
            SO_CraftingRecipe craftingRecipe = craftingRecipes[i].craftingRecipe;

            if (craftingRecipe == null) { break; }
            if (craftingRecipe.resultItem.item.itemType == itemTypes) 
            {
                recipes.Add(craftingRecipe);
            }
        }

        return recipes;
    }
}
