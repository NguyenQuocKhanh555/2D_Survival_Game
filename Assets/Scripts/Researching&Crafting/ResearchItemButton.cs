using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchItemButton : MonoBehaviour
{ 
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;
    [SerializeField] private Image _itemIconImage;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private GameObject _researchButton;
    
    public SO_ItemContainer researchedItemContainer;
    public ItemSlot itemSlot = new ItemSlot();

    public void OnClickResearchItemButton()
    {
        _itemDragAndDrop.OnClickOnResearchTablePanel(itemSlot);
        UpdateResearchInfo();
        _researchButton.SetActive(!researchedItemContainer.CheckItem(itemSlot));
    }

    public void ReturnItemInSlot()
    {
        if (itemSlot.item == null) return;

        InventoryManager.instance.AddItemToInventory(itemSlot.item, itemSlot.quantity);
        itemSlot.Clear();
        UpdateResearchInfo();
    }

    public void UpdateResearchInfo()
    {
        if (itemSlot.item != null)
        {
            _itemIconImage.sprite = itemSlot.item.itemIcon;
            _itemIconImage.gameObject.SetActive(true);
            _itemDescription.text = itemSlot.item.itemName + "\n" + itemSlot.item.itemDescription;
            _itemDescription.gameObject.SetActive(true);
        }
        else
        {
            _itemIconImage.sprite = null;
            _itemIconImage.gameObject.SetActive(false);
            _itemDescription.text = string.Empty;
            _itemDescription.gameObject.SetActive(false);
        }
    }
}
