using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float timeToLive = 5f;

    public SO_Item item;
    public int amount;

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Set(SO_Item item, int amount)
    {
        this.item = item;
        this.amount = amount;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.itemIcon;
    }
}
