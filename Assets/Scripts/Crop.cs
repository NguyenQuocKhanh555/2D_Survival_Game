using UnityEngine;
using UnityEngine.Tilemaps;

public class Crop : MonoBehaviour
{
    public SO_Crop cropData;
    //public int growTimer;
    public int growState;
    public SpriteRenderer spriteRenderer;
    public Vector3Int position;
    //public TileBase tileBeforePlow;
    public bool isWater;
    public float nextGrowthTime;

    public bool IsFullyGrown
    {
        get
        {
            if (cropData == null) return false;
            return growState >= cropData.growthStateTime.Length - 1;
        }
    }
}
