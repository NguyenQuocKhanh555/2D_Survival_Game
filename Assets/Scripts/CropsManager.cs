using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsManager : MonoBehaviour
{
    [SerializeField] private TileBase _plowedTile;
    [SerializeField] private TileBase _wateredTile;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _plowTilemap;
    [SerializeField] private Tilemap _wateringTilemap;
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private GameObject _cropPrefab;

    private SortedList<float, List<Crop>> _growthSortedList = new SortedList<float, List<Crop>>();
    private List<Crop> _cropOnMap = new List<Crop>();

    private void Start()
    {
        TimeController.instance.onTimeTick += GrowCheck;
        //VisualizeMap();
    }

    private void OnDestroy()
    {
        TimeController.instance.onTimeTick -= GrowCheck;
    }

    private void GrowCheck()
    {
        float currentPhase = TimeController.instance.GetTime();

        while (_growthSortedList.Count > 0)
        {
            float firstEntry = _growthSortedList.Keys[0];
            if (firstEntry > currentPhase) break;

            List<Crop> cropsToGrow = _growthSortedList[firstEntry];
            _growthSortedList.RemoveAt(0);

            foreach (Crop crop in cropsToGrow)
            {
                Grow(crop);
            }
        }
    }

    private void Grow(Crop crop)
    {
        if (crop.IsFullyGrown || crop.cropData == null) return;

        crop.growState++;
        crop.spriteRenderer.sprite = crop.cropData.sprites[crop.growState];
        crop.isWater = false;
    }

    private void ScheduleCrop(Crop crop, float growthTime)
    {
        if (!_growthSortedList.ContainsKey(growthTime))
        {
            _growthSortedList[growthTime] = new List<Crop>();
        }

        _growthSortedList[growthTime].Add(crop);
    }

    private void VisualizeMap()
    {
/*        for (int i = 0; i < cropContainer.crops.Count; i++)
        {
            VisualizeCrop(cropContainer.crops[i]);
        }*/
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

        crop.spriteRenderer.sprite = crop.cropData.sprites[crop.growState];
    }

    public bool Check(Vector3Int position)
    {
        foreach (Crop crop in _cropOnMap)
        {
            if (crop.position == position)
            {
                return true;
            }
        }

        return false;
    }

    public Crop GetCrop(Vector3Int position)
    {
        foreach (Crop crop in _cropOnMap)
        {
            if (crop.position == position)
            {
                return crop;
            }
        }

        return null;
    }

    public void Plow(Vector3Int position)
    {
        if (Check(position))
        {
            Crop crop = GetCrop(position);
            _cropOnMap.Remove(crop);
            Destroy(crop.gameObject);
            _plowTilemap.SetTile(position, null);
            _wateringTilemap.SetTile(position, null);
            return;
        }

        CreatePlowedTile(position);
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        GameObject go = Instantiate(_cropPrefab, transform);
        go.transform.position = _tilemap.CellToWorld(position);
        go.transform.position -= Vector3.forward * 0.01f;
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);

        Crop crop = go.GetComponent<Crop>();
        _cropOnMap.Add(crop);
        crop.position = position;
        crop.spriteRenderer = go.GetComponent<SpriteRenderer>();

        go.SetActive(false);

        _plowTilemap.SetTile(position, _plowedTile);
    }

    public void Seed(Vector3Int position, SO_Crop toSeed)
    {
        Crop crop = GetCrop(position);

        if (crop == null) return;
        if (crop.cropData != null) return;

        crop.cropData = toSeed;
        crop.growState = 0;
        crop.gameObject.SetActive(true);
        crop.spriteRenderer.sprite = toSeed.sprites[0];

        if (!crop.isWater) return;
        crop.nextGrowthTime = TimeController.instance.GetTime() + toSeed.growthStateTime[crop.growState + 1];
        ScheduleCrop(crop, crop.nextGrowthTime);
    }

    public void Watering(Vector3Int position)
    {
        Crop crop = GetCrop(position);

        if (crop == null) return;
        if (crop.isWater) return;

        crop.isWater = true;
        _wateringTilemap.SetTile(position, _wateredTile);

        if (crop.cropData == null) return;
        crop.nextGrowthTime = TimeController.instance.GetTime() + crop.cropData.growthStateTime[crop.growState + 1];
        ScheduleCrop(crop, crop.nextGrowthTime);
    }
}
