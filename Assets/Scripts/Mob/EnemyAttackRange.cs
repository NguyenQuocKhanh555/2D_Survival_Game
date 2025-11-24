using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public Vector2 playerPosition;
    public bool isPlayerInAttackRange = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;
            isPlayerInAttackRange = true;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = Vector2.zero;
            isPlayerInAttackRange = false;
        }
    }
}
