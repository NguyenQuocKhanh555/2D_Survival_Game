using System.Collections.Generic;
using UnityEngine;

public class CraftMaterialsPanel : MonoBehaviour
{
    [SerializeField] private List<CraftMaterialInfosPanel> _craftMaterialInfos;
    [SerializeField] private RectTransform _craftMaterialsPanelContent;

    public CraftingPanel craftingPanel;

    public void ShowCraftMaterialsInfos(List<ItemSlot> materials, int craftQuantity)
    {
        for (int i = 0; i < _craftMaterialInfos.Count; i++) 
        {
            _craftMaterialInfos[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < materials.Count; i++)
        {
            _craftMaterialInfos[i].ShowMaterialInfos(materials[i], craftQuantity);
        }

        _craftMaterialsPanelContent.sizeDelta = new Vector2(_craftMaterialsPanelContent.sizeDelta.x, 128 * materials.Count);
    }
}
