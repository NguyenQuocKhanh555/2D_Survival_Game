using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Converting Recipe", menuName = "Converting Recipe")]
public class SO_ConvertingRecipe : ScriptableObject
{
    public int convertingRecipeID;
    public List<ItemSlot> materials;
    public ItemSlot result;
    public int timeToProcess;
}
