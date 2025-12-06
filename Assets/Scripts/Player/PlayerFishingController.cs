using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFishingController : MonoBehaviour
{
    [SerializeField] private Transform _fishingBobberTransform;
    [SerializeField] private List<SO_FishPool> fishPools;
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private SO_ItemContainer _inventoryContainer;
 
    public void StartFishing()
    {
        Vector3Int gridPosition = _tilemapReadController.GetGridPosition(_fishingBobberTransform.position, false);
        TileBase waterTile = _tilemapReadController.GetTileAtGridPosition(gridPosition);

        SO_FishPool pool = GetPool(waterTile);

        if (pool == null) { return; }

        StartCoroutine(FishingCoroutine(pool));
    }

    private SO_FishPool GetPool(TileBase waterTile)
    {
        foreach (SO_FishPool pool in fishPools)
        {
            if (pool.waterTile == waterTile)
            {
                return pool;
            }
        }
        return null;
    }

    private IEnumerator FishingCoroutine(SO_FishPool pool)
    {
        float waitTime = Random.Range(30f, 60f);
        SO_Item fish = GetFish(pool.availableFish);

        yield return new WaitForSeconds(waitTime);

        _inventoryContainer.AddItem(fish, 1);
    }

    private SO_Item GetFish(List<Fish> fishes)
    {
        float totalWeight = 0f;
        foreach (Fish fish in fishes)
        {
            totalWeight += 1f / Mathf.Max(1, fish.rarity);
        }

        float randomValue = Random.value * totalWeight;

        float cumulativeWeight = 0f;
        foreach (Fish fish in fishes)
        {
            float weight = 1f / Mathf.Max(1, fish.rarity);
            cumulativeWeight += weight;
            if (randomValue <= cumulativeWeight)
            {
                return fish.fishItem;
            }
        }

        return fishes[fishes.Count - 1].fishItem;
    }
}
