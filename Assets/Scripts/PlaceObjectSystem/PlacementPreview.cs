using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementPreview : MonoBehaviour
{
    [SerializeField] private Tilemap _targetTilemap;

    private Vector3Int _cellPosition;
    private Vector3 _targetPosition;
    private SpriteRenderer _spriteRenderer;
    private bool _canSelect;
    private bool _show;
    
    public bool CanSelect
    {
        set
        {
            _canSelect = value;
            gameObject.SetActive(_canSelect && _show);
        }
    }

    public bool Show
    {
        set
        {
            _show = value;
            gameObject.SetActive(_canSelect && _show);
        }
    }

    private void Update()
    {
        _targetPosition = _targetTilemap.CellToWorld(_cellPosition);
        transform.position = _cellPosition;
    }

    private void Set(Sprite placementPreviewSprite)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.sprite = placementPreviewSprite;
    }
}
