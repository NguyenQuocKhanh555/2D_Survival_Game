using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    private List<Interactable> _interactablesInRange = new List<Interactable>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null && !_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null && _interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Remove(interactable);
        }
    }

    public List<Interactable> GetInteractablesInRange()
    {
        return _interactablesInRange;
    }
}
