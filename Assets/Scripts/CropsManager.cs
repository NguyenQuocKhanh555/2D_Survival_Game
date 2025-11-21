using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    public SO_CropContainer cropContainer;
    
    [SerializeField] private TileBase _plowedTile;
    [SerializeField] private TileBase _wateredTile;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private GameObject _cropPrefab;

    private void Start()
    {
        TimeController.instance.onTimeTick += Grow;
        VisualizeMap();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < cropContainer.crops.Count; i++)
        {
            cropContainer.crops[i].spriteRenderer = null;
        }
    }

    private void Grow()
    {
        foreach (Crop crop in cropContainer.crops)
        {
            if (crop.cropData == null) continue;
            if (crop.IsFullyGrown) continue;

            if (crop.isWater)
                crop.growTimer += 1;

            if (crop.growTimer >= crop.cropData.growthStateTime[crop.growState + 1])
            {     
                crop.growState += 1;
                crop.spriteRenderer.sprite = crop.cropData.sprites[crop.growState];
                crop.isWater = false;
                _tilemap.SetTile(crop.position, _plowedTile);
            }  
        }
    }

    private void VisualizeMap()
    {
        for (int i = 0; i < cropContainer.crops.Count; i++)
        {
            VisualizeCrop(cropContainer.crops[i]);
        }
    }

    private void VisualizeCrop(Crop crop)
    {
        _tilemap.SetTile(crop.position, crop.isWater ? _wateredTile : _plowedTile);

        if (crop.spriteRenderer == null)
        {
            GameObject go = Instantiate(_cropPrefab, transform);
            go.transform.position = _tilemap.CellToWorld(crop.position);
            go.transform.position -= Vector3.forward * 0.01f;
            crop.spriteRenderer = go.GetComponent<SpriteRenderer>();
        }

        if (crop.cropData == null)
        {
            crop.spriteRenderer.gameObject.SetActive(false);
            return;
        }

        for (int i = 1; i < crop.cropData.growthStateTime.Length; i++)
        {
            if (crop.growTimer < crop.cropData.growthStateTime[i]) break;

            crop.growState = i;
        }

        crop.spriteRenderer.sprite = crop.cropData.sprites[crop.growState];
    }

    public bool Check(Vector3Int position)
    {
        return cropContainer.Get(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if (Check(position))
        {
            Crop crop = cropContainer.Get(position);
            cropContainer.Remove(crop);
            Destroy(crop.spriteRenderer.gameObject);
            _tilemap.SetTile(position, crop.tileBeforePlow);
            return;
        }

        CreatePlowedTile(position);
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        Crop crop = new Crop();
        cropContainer.Add(crop);
        crop.position = position;
        crop.tileBeforePlow = _tilemapReadController.GetTileAtGridPosition(position);

        VisualizeCrop(crop);

        _tilemap.SetTile(position, _plowedTile);
    }

    public void Seed(Vector3Int position, SO_Crop toSeed)
    {
        Crop crop = cropContainer.Get(position);

        if (crop == null) return;
        if (crop.cropData != null) return;

        crop.cropData = toSeed;
        crop.growTimer = 0;
        crop.growState = 0;
        crop.spriteRenderer.gameObject.SetActive(true);
        crop.spriteRenderer.sprite = toSeed.sprites[0];
    }

    public void Watering(Vector3Int position)
    {
        Crop crop = cropContainer.Get(position);

        if (crop == null) return;
        if (crop.isWater) return;

        crop.isWater = true;
        _tilemap.SetTile(position, _wateredTile);
    }
}
