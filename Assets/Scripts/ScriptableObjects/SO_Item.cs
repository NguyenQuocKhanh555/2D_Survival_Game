using UnityEngine;

public enum ItemTypes
{
    Tool,
    Weapon,
    Armor,
    Consumable,
    Material,
    Placeable,
    Seed,
    WateringCan
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public bool isStackable;
    public int maxStack;
    public Sprite itemIcon;
    public ItemTypes itemType;
    public string itemDescription;
    public SO_Tool toolData;
    public SO_Weapon weaponData;
    public SO_ItemEffect itemEffect;
    public SO_PlayerBodyPart armorData;
    public SO_PlaceableObject placeableData;
    public SO_Crop cropData;
}
