using UnityEngine;

public class ResearchTableInteract : Interactable
{
    private GameObject _researchingUI;

    public override void Interact(Player player)
    {
        if (_researchingUI == null)
        {
            _researchingUI = player.GetComponent<PlayerUIInteractController>().researchTableUI;
        }
        _researchingUI.SetActive(!_researchingUI.activeSelf);
    }
}
