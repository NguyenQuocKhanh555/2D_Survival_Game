using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public SO_Item placeableItem;
    public Transform targetObject;
    //public Vector3Int positionOnGrid;
    public List<Vector3Int> occupiedGridPositions;
    public string objectState;

    public PlaceableObject(SO_Item item/*, Vector3Int position*/, List<Vector3Int> occupiedPosition)
    {
        placeableItem = item;
        //positionOnGrid = position;
        occupiedGridPositions = new List<Vector3Int>(occupiedPosition);
    }

    public bool OccupiesPosition(Vector3Int position)
    {
        return occupiedGridPositions.Contains(position);
    }
}

[CreateAssetMenu(fileName = "New Placeable Objects Container", menuName = "Placeable Objects Container")]
public class SO_PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjectsList;

    public void AddPlaceableObject(PlaceableObject placeableObject)
    {
        placeableObjectsList.Add(placeableObject);
    }

    public PlaceableObject GetPlaceableObject(Vector3Int position)
    {
        return placeableObjectsList.Find(obj => obj.OccupiesPosition(position));
    }

    public void RemovePlaceableObject(PlaceableObject placeableObject)
    {
        placeableObjectsList.Remove(placeableObject);
    }
}
