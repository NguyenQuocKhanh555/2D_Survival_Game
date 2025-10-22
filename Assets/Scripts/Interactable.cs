using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private Material[] _highlightMats;

    private List<Material> _originalMats = new List<Material>();

    private bool _isSelected = false;

    private void Awake()
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _originalMats.Add(_spriteRenderers[i].material);
        }
    }

    public void Select()
    {
        if (!_isSelected)
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                if (_spriteRenderers[i] != null && _highlightMats[i] != null)
                {
                    _spriteRenderers[i].material = _highlightMats[i];
                }
            }
            _isSelected = true;
        }
    }

    public void Deselect()
    {
        if (_isSelected)
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                if (_spriteRenderers[i] != null && _originalMats[i] != null)
                {
                    _spriteRenderers[i].material = _originalMats[i];
                }
            }
            _isSelected = false;
        }
    }

    public virtual void Interact(Player player)
    {
        
    }
}
