using UnityEngine;

public class SpawnItemManager : MonoBehaviour
{
    [SerializeField] private GameObject _pickupItemPrefab;

    public static SpawnItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItem(Vector3 position, SO_Item item, int quantity)
    {
        GameObject pickupItemObject = Instantiate(_pickupItemPrefab, position, Quaternion.identity);
        pickupItemObject.GetComponent<PickupItem>().Set(item, quantity);
    }
}
