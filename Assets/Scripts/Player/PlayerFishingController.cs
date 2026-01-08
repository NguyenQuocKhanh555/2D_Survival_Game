using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFishingController : MonoBehaviour
{
    [SerializeField] private List<SO_FishPool> fishPools;
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private FishingPanel _fishingPanel;
    [SerializeField] private GameObject _alert;

    public Vector2 lastMotionVector;
    public bool isFishing = false;
    public bool hasCaughtFish = false;
    public bool isReeling = false;

    private Animator _animator;
    private SO_Item _caughtFishItem;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _fishingPanel.fishingResult += OnFishingResult;
    }

    public void StartFishing()
    {
        Vector3Int gridPosition = _tilemapReadController.GetGridPosition((Vector2)transform.position + lastMotionVector, false);
        TileBase waterTile = _tilemapReadController.GetTileAtGridPosition(gridPosition);

        SO_FishPool pool = GetPool(waterTile);

        if (pool == null) { 
            _animator.SetBool("isCancel", true);
            return; 
        }

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
        isFishing = true;
        float waitTime = UnityEngine.Random.Range(30f, 60f);
        _caughtFishItem = GetFish(pool.availableFish);

        yield return new WaitForSeconds(waitTime);

        _alert.SetActive(true);
        hasCaughtFish = true;

        yield return new WaitForSeconds(5f);

        _caughtFishItem = null;
        _alert.SetActive(false);
        hasCaughtFish = false;
    }

    private SO_Item GetFish(List<Fish> fishes)
    {
        float totalWeight = 0f;
        foreach (Fish fish in fishes)
        {
            totalWeight += 1f / Mathf.Max(1, fish.rarity);
        }

        float randomValue = UnityEngine.Random.value * totalWeight;

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

    public void StopFishing()
    {
        isFishing = false;
        hasCaughtFish = false;
        _caughtFishItem = null;
        StopAllCoroutines();
    }

    public void Reel()
    {
        _alert.SetActive(false);
        isReeling = true;
        _fishingPanel.gameObject.SetActive(true);
        StopAllCoroutines();
    }

    private void OnFishingResult(bool result)
    {
        isReeling = false;
        hasCaughtFish = false;
        isFishing = false;
        _animator.SetBool("isReel", false);

        if (result)
        {
            InventoryManager.instance.AddItemToInventory(_caughtFishItem, 1);
        }
        
        _caughtFishItem = null;
    }
}
