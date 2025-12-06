using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Fish
{
    public SO_Item fishItem;
    public int rarity;
}

[CreateAssetMenu(fileName = "New Fish Pool", menuName = "Fish Pool")]
public class SO_FishPool : ScriptableObject
{
    public TileBase waterTile;
    public List<Fish> availableFish;
}
