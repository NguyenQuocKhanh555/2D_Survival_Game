using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private float _offsetDistance = 1.0f;
    [SerializeField] private Vector2 _attackAreaSize = new Vector2(1.0f, 1.0f);

    public void MeleeAttack(float damage, Vector2 lastMotionVector)
    {
        Vector2 position = (Vector2)transform.position + lastMotionVector * _offsetDistance;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, _attackAreaSize, 0f);

        foreach (Collider2D collider in hitColliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    } 
}
