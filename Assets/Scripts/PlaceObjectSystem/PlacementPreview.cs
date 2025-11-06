using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementPreview : MonoBehaviour
{
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerToolbarController _playerToolbarController;

    
    private Vector3 _targetPosition;
    private bool _canSelect;
    private bool _show;

    public Vector3Int cellPosition;

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

    private void Start()
    {
        _playerToolbarController.onToolbarSelectedChanged += UpdatePlacementPreview;
        UpdatePlacementPreview();
    }

    private void Update()
    {
        _targetPosition = _targetTilemap.CellToWorld(cellPosition);
        transform.position = cellPosition;
    }

    private void UpdatePlacementPreview()
    {
        SO_Item item = _playerToolbarController.GetToolbarSelectedItem;

        if (item == null || item.itemType != ItemTypes.Placeable)
        {
            Show = false;
            return;
        }
        
        Show = true;
        _spriteRenderer.sprite = item.placeableData.spritePlacementPreview;
    }
}
