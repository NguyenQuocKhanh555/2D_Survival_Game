using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    private Interactable currentSelectedObject = null;

    [SerializeField] private InteractDetector interactDetector;

    public bool isInteracting = false;

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        var interactable = interactDetector.GetInteractablesInRange();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);
        Interactable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            Interactable hit = collider.GetComponent<Interactable>();
            if (hit != null && interactable.Contains(hit))
            {
                isInteracting = true;
                float distance = Vector2.Distance(mousePosition, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = hit;
                }
            }
        }

        if (closestInteractable != null && closestInteractable != currentSelectedObject)
        {
            if (currentSelectedObject != null)
            {
                currentSelectedObject.Deselect();
            }
            currentSelectedObject = closestInteractable;
            currentSelectedObject.Select();
        }
        else if (closestInteractable == null && currentSelectedObject != null)
        {
            currentSelectedObject.Deselect();
            currentSelectedObject = null;
            isInteracting = false;
        }
    }

    public void Interact(Player player)
    {
        if (currentSelectedObject == null) { return; }

        currentSelectedObject.Interact(player);
    }
}
