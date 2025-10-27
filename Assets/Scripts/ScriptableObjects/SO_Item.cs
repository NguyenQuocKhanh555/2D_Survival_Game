using UnityEngine;

public enum ItemTypes
{
    Tool,
    Weapon,
    Armor,
    Consumable,
    Material
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public bool isStackable;
    public Sprite itemIcon;
    public ItemTypes itemType;
    public SO_Tool toolData;
}
