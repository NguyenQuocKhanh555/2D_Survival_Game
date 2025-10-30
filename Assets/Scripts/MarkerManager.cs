using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private TileBase _markerTile;

    private Vector3Int _oldTargetCellPosition;
    private bool _isShowingMarker;

    public Vector3Int markedCellPosition;

    private void Update()
    {
        if (!_isShowingMarker) { return; }
        _targetTilemap.SetTile(_oldTargetCellPosition, null);
        _targetTilemap.SetTile(markedCellPosition, _markerTile);
        _oldTargetCellPosition = markedCellPosition;
    }

    public void ShowMarker(bool isSelectable)
    {
        _isShowingMarker = isSelectable;
        _targetTilemap.gameObject.SetActive(isSelectable);
    }
}
