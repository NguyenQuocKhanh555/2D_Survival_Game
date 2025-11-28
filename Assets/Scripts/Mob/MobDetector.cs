using UnityEngine;

public class MobDetector : MonoBehaviour
{
    private Vector2 _playerPosition;
    public bool isPlayerInDectectRange = false;
    
    public Vector2 MobToPlayer
    {
        get
        {
            return (_playerPosition - (Vector2)(transform.position)).normalized;
        }
    }
    
    public Vector2 PlayerToMob
    {
        get
        {
            return ((Vector2)(transform.position) - _playerPosition).normalized;
        }
    }

    public float DistanceToPlayer
    {
        get
        {
            return Vector2.Distance((Vector2)transform.position, _playerPosition);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerPosition = collision.transform.position;
            isPlayerInDectectRange = true;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerPosition = Vector2.zero;
            isPlayerInDectectRange = false;
        }
    }
}
