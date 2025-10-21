using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Material[] highlightMats;

    private List<Material> originalMats = new List<Material>();

    private bool _isSelected = false;

    private void Awake()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalMats.Add(spriteRenderers[i].material);
        }
    }

    public void Select()
    {
        if (!_isSelected)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null && highlightMats[i] != null)
                {
                    spriteRenderers[i].material = highlightMats[i];
                }
            }
            _isSelected = true;
        }
    }

    public void Deselect()
    {
        if (_isSelected)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null && originalMats[i] != null)
                {
                    spriteRenderers[i].material = originalMats[i];
                }
            }
            _isSelected = false;
        }
    }

    public virtual void Interact(Player player)
    {
        
    }
}
