using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupItemController : MonoBehaviour
{
    [SerializeField] private float _pickupRange = 1.5f;

    [SerializeField] private Vector2 _directionToPickupItem;
    [SerializeField] private Transform _targetItem;
    [SerializeField] private Transform _lastNearestItem;

    public bool isMovingToPickupItem;

    public void PickupSingleItem()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("PickupItem"))
        {
            _targetItem = hit.collider.transform;
            isMovingToPickupItem = true;
            _directionToPickupItem = (_targetItem.position - transform.position).normalized;
        }
    }

    public void FindNearestPickupItem()
    {
        if (_targetItem != null) return;
        GameObject[] pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");

        float nearestDistance = Mathf.Infinity;
        _lastNearestItem = null;

        foreach (GameObject item in pickupItems)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                _lastNearestItem = item.transform;
            }
        }

        SetTargetPickupItem();
    }

    public void SetTargetPickupItem()
    {
        _targetItem = _lastNearestItem;
        
        if (_targetItem == null) return;
        
        isMovingToPickupItem = true;
        _directionToPickupItem = (_targetItem.position - transform.position).normalized;
    }

    public void MoveToTargetPickupItem(float walkSpeed, Animator animator)
    {
        if (isMovingToPickupItem && _targetItem != null)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                _targetItem.position,
                walkSpeed * Time.fixedDeltaTime);

            float distance = Vector2.Distance(transform.position,
                _targetItem.position);

            if (distance <= _pickupRange)
            {
                isMovingToPickupItem = false;
                PickupItem(animator);
            }
        }
    }

    public void SetStateToMoving(Animator animator)
    {
        if (!isMovingToPickupItem) return;
        animator.SetBool("moving", true);
        animator.SetFloat("horizontal", _directionToPickupItem.x);
        animator.SetFloat("vertical", _directionToPickupItem.y);
    }

    public void PickupItem(Animator animator)
    {
        SO_Item pickupItem = _targetItem.GetComponent<PickupItem>().item;
        int amount = _targetItem.GetComponent<PickupItem>().amount;

        if (_targetItem == null) return;

        animator.SetFloat("lastHorizontal", _directionToPickupItem.x);
        animator.SetFloat("lastVertical", _directionToPickupItem.y);
        animator.SetTrigger("pickupItem");

        InventoryManager.instance.AddItemToInventory(pickupItem, amount);
        Destroy(_targetItem.gameObject);
        _targetItem = null;
    }
}
