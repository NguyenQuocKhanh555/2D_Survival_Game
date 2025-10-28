using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceNode : ToolHit
{
    [SerializeField] private GameObject _pickupItemDrop;
    [SerializeField] private float _spread = 0.5f;
    [SerializeField] private SO_Item _item;
    [SerializeField] private int _itemCountInOneDrop = 1;
    [SerializeField] private int _dropCount = 5;
    [SerializeField] private ResourceNodeType _nodeType;
    [SerializeField] private float _resourceNodeDurability = 100f;

    public override void Hit(float toolPower)
    {
        _resourceNodeDurability -= toolPower;

        if (_resourceNodeDurability > 0) return;

        while (_dropCount > 0)
        {
            _dropCount--;

            Vector3 position = transform.position;
            position.x += _spread * UnityEngine.Random.value - _spread / 2;
            position.y += _spread * UnityEngine.Random.value - _spread / 2;
        }

        Destroy(gameObject);
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(_nodeType);
    }
}
