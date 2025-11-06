using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObjectsManager : MonoBehaviour
{
    [SerializeField] private SO_PlaceableObjectsContainer _placeableObjectsContainer;
    [SerializeField] private Tilemap _targetTilemap;

    private void Start()
    {
        VisualizeMap();
    }

    private void VisualizeMap()
    {
        for (int i = 0; i < _placeableObjectsContainer.placeableObjectsList.Count; i++)
        {
            VisualizeItem(_placeableObjectsContainer.placeableObjectsList[i]);
        }
    }

    private void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject obj =  Instantiate(placeableObject.placeableItem.placeableData.placeableItemPrefab);
        obj.transform.parent = this.transform;

        if (placeableObject.occupiedGridPositions.Count > 0)
        {
            Vector3 worldPosition = _targetTilemap.CellToWorld(placeableObject.occupiedGridPositions[0]);
            worldPosition -= Vector3.forward * 0.1f;
            obj.transform.position = worldPosition;
        }

        IPersistant persistant = obj.GetComponent<IPersistant>();
        if (persistant != null)
        {
            persistant.Load(placeableObject.objectState);
        }

        placeableObject.targetObject = obj.transform;
    }

    public bool Check(List<Vector3Int> occupiedPositions)
    {
        foreach (var pos in occupiedPositions)
        {
            if (_placeableObjectsContainer.GetPlaceableObject(pos) != null)
                return true;
        }
        return false;
    }

    public void Place(SO_Item item, List<Vector3Int> occupiedPositions)
    {
        if (Check(occupiedPositions)) return;

        PlaceableObject newPlaceableObject = new PlaceableObject(item, occupiedPositions);
        _placeableObjectsContainer.AddPlaceableObject(newPlaceableObject);
        VisualizeItem(newPlaceableObject);
    }
}
