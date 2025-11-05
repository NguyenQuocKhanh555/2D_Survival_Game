using UnityEngine;

public class ResearchTableInteract : Interactable
{
    [SerializeField] private ResearchTablePanel _researchingUI;

    public override void Interact(Player player)
    {
        _researchingUI.gameObject.SetActive(true);
    }
}
