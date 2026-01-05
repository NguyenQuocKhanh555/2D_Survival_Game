using UnityEngine;

public class ResearchTableInteract : Interactable
{
    private GameObject _researchingUI;
    private ResearchItemButton _researchItemButton;

    public override void Interact(Player player)
    {
        if (_researchingUI == null)
        {
            _researchingUI = player.GetComponent<PlayerUIInteractController>().researchTableUI;
        }
        _researchingUI.SetActive(!_researchingUI.activeSelf);
        if (!_researchingUI.activeSelf)
        {
            if (_researchItemButton == null)
            {
                _researchItemButton = _researchingUI.GetComponentInChildren<ResearchItemButton>();
            }
            _researchItemButton.ReturnItemInSlot();
        }
    }
}
