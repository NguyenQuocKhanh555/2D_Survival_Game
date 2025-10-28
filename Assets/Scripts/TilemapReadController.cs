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

    public TileBase GetTileAtGridPosition(Vector3Int gridPosition)
    {
        return _tilemap.GetTile(gridPosition);
    }
}
