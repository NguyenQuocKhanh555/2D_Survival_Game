using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Crop
{
    public SO_Crop cropData;
    public int growTimer;
    public int growState;
    public SpriteRenderer spriteRenderer;
    public Vector3Int position;
    public TileBase tileBeforePlow;
    public bool isWater;

    public bool IsFullyGrown
    {
        get
        {
            if (cropData == null) return false;
            return growTimer >= cropData.timeToGrow;
        }
    }

    public void Harvested()
    {
        growTimer = 0;
        cropData = null;
        spriteRenderer.gameObject.SetActive(false);
    }
}

[CreateAssetMenu(fileName = "New Crop Container", menuName = "Crop/Crop Container")]
public class SO_CropContainer : ScriptableObject
{
    public List<Crop> crops;

    public Crop Get(Vector3Int position)
    {
        return crops.Find(x => x.position == position);
    }

    public void Add(Crop crop)
    {
        crops.Add(crop);
    }

    public void Remove(Crop crop)
    {
        crops.Remove(crop);
    }

    public void Clear()
    {
        crops.Clear();
    }
}
