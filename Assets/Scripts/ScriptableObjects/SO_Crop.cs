using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Crop/Crop")]
public class SO_Crop : ScriptableObject
{
    public int cropId;
    public int timeToGrow;
    public Product[] products;
    public Sprite[] sprites;
    public int[] growthStateTime;

    public struct Product
    {
        public SO_Item productItem;
        public int quantity;
    }
}
