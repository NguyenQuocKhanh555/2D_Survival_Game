using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class SO_CraftingRecipe : ScriptableObject
{
    public ItemSlot[] craftMaterials;
    public ItemSlot resultItem;
}
