using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    private Interactable _currentSelectedObject = null;
    private Interactable _currentInteractObject = null;

    [SerializeField] private InteractDetector _interactDetector;

    public bool isInteracting = false;
    
    private void Update()
    {
        if (_currentInteractObject != null) return;
        Check();
    }

    private void Check()
    {
        var interactable = _interactDetector.GetInteractablesInRange();

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

        if (closestInteractable != null && closestInteractable != _currentSelectedObject)
        {
            if (_currentSelectedObject != null)
            {
                _currentSelectedObject.Deselect();
            }
            _currentSelectedObject = closestInteractable;
            _currentSelectedObject.Select();
        }
        else if (closestInteractable == null && _currentSelectedObject != null)
        {
            _currentSelectedObject.Deselect();
            _currentSelectedObject = null;
            isInteracting = false;
        }
    }

    public bool Interact(Player player)
    {
        if (_currentInteractObject != null)
        {
            _currentInteractObject.Interact(player);
            _currentInteractObject = null;
            return false;
        }

        if (_currentSelectedObject == null) { return false; }

        _currentSelectedObject.Interact(player);
        _currentInteractObject = _currentSelectedObject;
        return true;
    }
}
