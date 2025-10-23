using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public interface IItemButton
{
    public void Set(ItemSlot itemSlot);
    public void Clear();
}
