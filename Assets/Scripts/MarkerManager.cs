using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private TileBase _selectablemarkerTile;
    [SerializeField] private TileBase _nonSelectableMarkerTile;

    private Vector3Int _oldTargetCellPosition;
    private List<Vector3Int> _oldTargetCellArea = new List<Vector3Int>();
    private bool _isShowingMarker;

    public Vector3Int markedCellPosition;
    public List<Vector3Int> markedCellArea = new List<Vector3Int>();

    private void Update()
    {
        if (!_isShowingMarker)
            return;

        if (_oldTargetCellArea.Count > 0)
        {
            foreach (var oldCell in _oldTargetCellArea)
            {
                _targetTilemap.SetTile(oldCell, null);
            }
        }
        else
        {
            _targetTilemap.SetTile(_oldTargetCellPosition, null);
        }

        if (markedCellArea != null && markedCellArea.Count > 0)
        {
            foreach (var cell in markedCellArea)
            {
                _targetTilemap.SetTile(cell, _selectablemarkerTile);
            }
        }
        else
        {
            _targetTilemap.SetTile(markedCellPosition, _selectablemarkerTile);
        }

        _oldTargetCellArea = new List<Vector3Int>(markedCellArea);
        _oldTargetCellPosition = markedCellPosition;
    }

    public void ShowMarker(bool isSelectable)
    {
        _isShowingMarker = isSelectable;
        _targetTilemap.gameObject.SetActive(isSelectable);
    }
}
