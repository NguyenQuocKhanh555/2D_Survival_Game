using System;
using System.Collections;
using UnityEngine;

public enum MobType
{
    Animal,
    MeleeEnemy,
    RangeEnemy
}

public class Mob : MonoBehaviour
{
    [SerializeField] private float _mobAttackCooldown;
    [SerializeField] private float _mobSpecialCooldown;

    public EnemyAttackRange attackRange;
    public Animator animator;
    public Rigidbody2D rb;

    public Vector2 lastMotionVector = Vector2.down;
    public string mobName;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool canUseSpecial = true;
    public bool isUsingSpecial = false;    
    public MobType mobType;
    public int mobHealth;
    public float mobPatrolSpeed;
    public float mobRunSpeed;
    public GameObject projectilePrefab;

    private MobState _currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(new MobIdleState(this));
    }

    private void Update()
    {
        _currentState.Update();
    }

    public void ChangeState(MobState state)
    {
        if (_currentState != null) 
            _currentState.Exit();

        _currentState = state;
        _currentState.Enter();
    }

    public bool CanAttack()
    {
        return attackRange.isPlayerInAttackRange && canAttack && !isAttacking && !isUsingSpecial;
    }

    public bool ShadowMageCanUsingTeleport()
    {
        float distance = Vector2.Distance(attackRange.playerPosition, (Vector2)transform.position);
        bool isPlayerTooClose = distance <= 3f;
        return isPlayerTooClose && canUseSpecial && !isUsingSpecial && mobName == "ShadowMage" && !isAttacking; 
    }

    public void RangeAttack()
    {
        Vector2 projectileSpawnPoint = (Vector2)transform.position + lastMotionVector * 1.5f;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint, Quaternion.identity);
        Vector2 direction = (attackRange.playerPosition - projectileSpawnPoint).normalized;
        projectile.GetComponent<Projectile>().SetUp(direction);
        canAttack = false;
        StartCoroutine(AttackCooldowCoroutine());
    }

    private IEnumerator AttackCooldowCoroutine()
    {
        yield return new WaitForSeconds(_mobAttackCooldown);
        canAttack = true;
    }

    private IEnumerator SpecialCooldowCoroutine()
    {
        yield return new WaitForSeconds(_mobAttackCooldown);
        canAttack = true;
    }
}
