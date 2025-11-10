using UnityEngine;

public class ResearchButton : MonoBehaviour
{
    [SerializeField] private ResearchItemButton _researchItemButton;
    [SerializeField] private ResearchRecipePanel _researchRecipePanel;

    private ItemSlot _researchItemSlot = new ItemSlot();

    private void OnEnable()
    {
        _researchItemSlot.Copy(_researchItemButton.itemSlot);
    }

    public void OnClick()
    {
        _researchItemButton.researchedItem.AddItemSlot(_researchItemSlot);
        _researchItemButton.itemSlot.Clear();
        _researchItemButton.UpdateResearchInfo();
        _researchRecipePanel.OnClickResearchButton();
        this.gameObject.SetActive(false);
    }
}
