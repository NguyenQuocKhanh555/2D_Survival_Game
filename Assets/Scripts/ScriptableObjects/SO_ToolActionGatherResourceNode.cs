using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    Tree,
    Rock,
    Underfied
}

[CreateAssetMenu(fileName = "New Gather Resource Node", menuName = "Tool Action/Gather Resource Node")]
public class SO_ToolActionGatherResourceNode : SO_ToolAction
{
    [SerializeField] private float _sizeOfInteractableArea = 1f;
    [SerializeField] private List<ResourceNodeType> _canHitNodesOfType;

    public override void OnApply(Vector2 worldPoint, float toolPower)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPoint, _sizeOfInteractableArea);
        
        foreach (Collider2D collider in hitColliders)
        {
            ToolHit hit = collider.GetComponent<ToolHit>();
            if (hit != null)
            {
                if (hit.CanBeHit(_canHitNodesOfType))
                {
                    hit.Hit(toolPower);
                }
            }
        }
    }
}
