using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventorySlotController : MonoBehaviour

{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetIcon(Sprite newIcon)
    {
        icon.sprite = newIcon;
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
