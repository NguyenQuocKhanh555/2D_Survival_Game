using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{  
    [SerializeField] private Material _highlightMats;

    private SpriteRenderer _spriteRenderers;
    private Material _originalMats;

    private bool _isSelected = false;

    private void Awake()
    {
        _spriteRenderers = GetComponent<SpriteRenderer>();
        _originalMats = _spriteRenderers.material;
    }

    public void Select()
    {
        if (!_isSelected)
        {
            _spriteRenderers.material = _highlightMats;
            _isSelected = true;
        }
    }

    public void Deselect()
    {
        if (_isSelected)
        {
            _spriteRenderers.material = _originalMats;
            _isSelected = false;
        }
    }

    public virtual void Interact(Player player)
    {
        
    }
}
