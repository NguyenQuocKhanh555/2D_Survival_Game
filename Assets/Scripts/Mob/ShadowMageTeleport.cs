using System.Collections;
using UnityEngine;

public class ShadowMageTeleport : MonoBehaviour
{
    [SerializeField] private MobDetector _detector;
    [SerializeField] private GameObject _openPortal;
    [SerializeField] private GameObject _closePortal;
    [SerializeField] private float _teleportDistance;
    [SerializeField] private float _cooldown;
 
    private bool _canTeleport = true;
    private Vector2 _newPosition;
    
    public bool isTeleporting = false;

    public void CreateOpenPort()
    {
        _openPortal.SetActive(true);
        _closePortal.SetActive(true);
        Vector2 direction = _detector.PlayerToMob;
        _newPosition = (Vector2)transform.position + direction * _teleportDistance;
        _closePortal.transform.position = _newPosition;
    }

    public void Teleport()
    {
        _openPortal.SetActive(false);
        _canTeleport = false;
        transform.position = _newPosition;
        _closePortal.GetComponent<Animator>().SetTrigger("close");
        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _canTeleport = true;
    }

    public bool CanTeleport()
    {
        float distanceToPlayer = _detector.DistanceToPlayer;
        bool isPlayerToClose = distanceToPlayer <= 3f;
        return _canTeleport && isPlayerToClose && !isTeleporting;
    }
}
