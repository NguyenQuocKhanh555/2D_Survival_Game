using System.Collections.Generic;
using UnityEngine;

public class InteractDetector : MonoBehaviour
{
    private List<Interactable> interactablesInRange = new List<Interactable>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null && interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Remove(interactable);
        }
    }

    public List<Interactable> GetInteractablesInRange()
    {
        return interactablesInRange;
    }
}
