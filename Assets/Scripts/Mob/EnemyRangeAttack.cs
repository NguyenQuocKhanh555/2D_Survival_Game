using System.Collections;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _attackCooldown;

    private bool _canAttack = true;
    private Vector2 projectileSpawnPoint;
    private Vector2 direction;

    public bool isAttacking = false;
    public MobDetector attackRange;

    public void SetUp(Vector2 projectileSpawnPoint, Vector2 direction)
    {
        this.projectileSpawnPoint = projectileSpawnPoint;
        this.direction = direction;
    }

    public void RangeAttack()
    {
        GameObject projectile = Instantiate(_projectilePrefab, projectileSpawnPoint, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetUp(direction, _damageable);
        _canAttack = false;
        StartCoroutine(AttackCooldowCoroutine());
    }

    private IEnumerator AttackCooldowCoroutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public bool CanAttack()
    {
        return attackRange.isPlayerInDectectRange && _canAttack && !isAttacking;
    }
}
