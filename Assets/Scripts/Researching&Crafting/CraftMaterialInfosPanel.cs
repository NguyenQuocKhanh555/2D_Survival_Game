using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftMaterialInfosPanel : MonoBehaviour
{
    [SerializeField] private Image _materialIcon;
    [SerializeField] private TMP_Text _materialName;
    [SerializeField] private TMP_Text _materialCount;
    
    [SerializeField] private CraftMaterialsPanel _materialsPanel;

    private void OnEnable()
    {
        if (_materialsPanel != null) return;
        _materialsPanel = GetComponentInParent<CraftMaterialsPanel>();
    }

    public void ShowMaterialInfos(ItemSlot material, int craftQuantity)
    {
        this.gameObject.SetActive(true);
        _materialIcon.sprite = material.item.itemIcon;
        _materialName.text = material.item.name;

        int inventoryMaterialQuantity = InventoryManager.instance.GetItemQuantity(material.item);
        int materialQuantity = material.quantity * craftQuantity;
        _materialCount.text = inventoryMaterialQuantity + "/" + materialQuantity;
        _materialCount.color = inventoryMaterialQuantity >= materialQuantity ? Color.green : Color.red;
    }
}
