using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class SO_CraftingRecipe : ScriptableObject
{
    public List<ItemSlot> craftMaterials;
    public ItemSlot resultItem;
}
