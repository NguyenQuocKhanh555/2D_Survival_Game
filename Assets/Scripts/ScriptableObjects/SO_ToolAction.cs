using UnityEngine;

public class SO_ToolAction : ScriptableObject
{
    public virtual void OnApply(Vector2 worldPoint, float toolPower)
    {
        
    }

    public virtual void OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapRead)
    {

    }
}
