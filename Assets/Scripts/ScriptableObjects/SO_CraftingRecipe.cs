using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public SO_Item item;
    public int quantity;
}

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class SO_CraftingRecipe : ScriptableObject
{
    public ItemStack[] craftMaterials;
    public ItemStack resultItem;
}
