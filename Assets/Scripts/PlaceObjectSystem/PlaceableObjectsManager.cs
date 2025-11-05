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

        Vector3 worldPosition = _targetTilemap.CellToWorld(placeableObject.positionOnGrid);
        worldPosition -= Vector3.forward * 0.1f;
        obj.transform.position = worldPosition;

        IPersistant persistant = obj.GetComponent<IPersistant>();
        if (persistant != null)
        {
            persistant.Load(placeableObject.objectState);
        }

        placeableObject.targetObject = obj.transform;
    }

    public bool Check(Vector3Int position)
    {
        return _placeableObjectsContainer.GetPlaceableObject(position) != null;
    }

    public void Place(SO_Item item, Vector3Int position)
    {
        if (Check(position)) return;

        PlaceableObject newPlaceableObject = new PlaceableObject(item, position);
        _placeableObjectsContainer.AddPlaceableObject(newPlaceableObject);
        VisualizeItem(newPlaceableObject);
    }
}
