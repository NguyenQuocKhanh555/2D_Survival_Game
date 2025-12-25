using UnityEngine;

public class PlayerDeadController : MonoBehaviour
{
    [SerializeField] private GameObject _graveStonePrefab;
    [SerializeField] private TilemapReadController _tilemapReadController;

    public void CreateGraveStone()
    {
        Vector3 spawnPosition = _tilemapReadController.GetGridPosition(transform.position, false);
        GameObject go = Instantiate(_graveStonePrefab, spawnPosition, Quaternion.identity);
        go.GetComponent<GraveInteract>().StoreInventory();
    }  
}
