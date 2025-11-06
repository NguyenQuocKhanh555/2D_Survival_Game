using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapReadController : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = _tilemap.WorldToCell(worldPosition);

        return gridPosition;
    }

    public List<Vector3Int> GetGridArea(Vector2 position, bool mousePosition, Vector2Int size)
    {
        Vector3 worldPosition;

        if (mousePosition)
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        else
            worldPosition = position;

        Vector3Int originGrid = _tilemap.WorldToCell(worldPosition);
        List<Vector3Int> occupiedGrids = new List<Vector3Int>();

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector3Int gridPos = originGrid + new Vector3Int(x, y, 0);
                occupiedGrids.Add(gridPos);
            }
        }

        return occupiedGrids;
    }

    public TileBase GetTileAtGridPosition(Vector3Int gridPosition)
    {
        return _tilemap.GetTile(gridPosition);
    }
}
