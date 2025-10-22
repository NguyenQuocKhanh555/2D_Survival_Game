using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryScript : MonoBehaviour
{
    [SerializeField]
    private SO_Inventory playerInventory;
    [SerializeField]
    private InventorySlotController[] buttons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // loop through each button and set its icon and text based on the player's inventory
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetIcon(playerInventory.items[i].item.itemSprite);
            buttons[i].SetText(playerInventory.items[i].quantity.ToString());
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
