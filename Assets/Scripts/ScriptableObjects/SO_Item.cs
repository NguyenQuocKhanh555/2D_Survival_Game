using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public bool isStackable;
    public Sprite itemSprite;

    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Material
    }

    public ItemType itemType;
}
