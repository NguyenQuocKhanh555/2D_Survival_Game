using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackRange;
    [SerializeField] private Vector2 _attackAreaSize;
    [SerializeField] private float _attackDamage;
    [SerializeField] private Damageable _damageable;

    private bool _canAttack = true;

    public bool isAttacking = false;
    public MobDetector detectRange;

    private bool IsPlayerInAttackRange
    {
        get
        {
            return detectRange.isPlayerInDectectRange && detectRange.DistanceToPlayer <= _attackRange;
        }
    }

    public void MeleeAttack(Vector2 lastMotionVector)
    {
        _canAttack = false;

        Vector2 position = (Vector2)transform.position + lastMotionVector;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, _attackAreaSize, 0f);

        foreach (Collider2D collider in hitColliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null && damageable != _damageable)
            {
                damageable.TakeDamage(_attackDamage);
            }
        }

        StartCoroutine(AttackCooldowCoroutine());
    }

    private IEnumerator AttackCooldowCoroutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public bool CanAttack()
    {
        return IsPlayerInAttackRange && _canAttack && !isAttacking;
    }
}
