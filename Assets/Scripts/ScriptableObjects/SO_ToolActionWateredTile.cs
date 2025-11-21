using UnityEngine;

[CreateAssetMenu(fileName = "New Watered Tile", menuName = "Tool Action/Watered Tile")]
public class SO_ToolActionWateredTile : SO_ToolAction
{
    public override void OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapRead)
    {
        tilemapRead.cropsManager.Watering(gridPosition);
    }
}
