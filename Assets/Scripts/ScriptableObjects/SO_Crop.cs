using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Crop/Crop")]
public class SO_Crop : ScriptableObject
{
    public int cropId;
    public Product[] products;
    public Sprite[] sprites;
    public int[] growthStateTime;

    [System.Serializable]
    public struct Product
    {
        public SO_Item productItem;
        public int quantity;
    }
}
