using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Plow Tile", menuName = "Tool Action/Plow Tile")]
public class SO_ToolActionPlowTile : SO_ToolAction
{
    [SerializeField] private List<TileBase> _canPlow;

    public override void OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapRead)
    {
        TileBase targetTile = tilemapRead.GetTileAtGridPosition(gridPosition);
        
        if (!_canPlow.Contains(targetTile)) return;

        tilemapRead.cropsManager.Plow(gridPosition);
    }
}
