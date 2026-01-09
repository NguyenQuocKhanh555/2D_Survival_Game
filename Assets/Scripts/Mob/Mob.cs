using System;
using System.Collections;
using UnityEngine;

public enum MobType
{
    Animal,
    MeleeEnemy,
    RangeEnemy
}

public class Mob : MonoBehaviour, IDamageable
{
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public MobDetector detector;

    public EnemyRangeAttack rangeAttack;
    public EnemyMeleeAttack meleeAttack;
    public ShadowMageTeleport teleport;
    public SO_Item dropItem;

    public Vector2 lastMotionVector = Vector2.down;  
    public MobType mobType;
    public float mobHealth;
    public float mobPatrolSpeed;
    public float mobRunSpeed;
    public bool canTakeDamage = true;
    public bool isDead = false;
    public bool isHit = false;

    private MobState _currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rangeAttack = GetComponent<EnemyRangeAttack>();
        meleeAttack = GetComponent<EnemyMeleeAttack>();
        teleport = GetComponent<ShadowMageTeleport>();

        _currentState = new MobIdleState(this);
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState.Update();

        if (isDead)
        {
            ChangeState(new MobDeathState(this));
        }
    }

    public void ChangeState(MobState state)
    {
        //if (GameManager.instance.CurrentGameState != GameState.Playing) return;

        if (_currentState != null) 
            _currentState.Exit();

        _currentState = state;
        _currentState.Enter();
    }

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;
        if (isDead) return;

        mobHealth -= damage;
        StartCoroutine(HitCoroutine());

        if (mobHealth <= 0)
        {
            isDead = true;
        }
    }

    public IEnumerator HitCoroutine()
    {
        spriteRenderer.color = Color.gray;
        isHit = true;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        isHit = false;
    }

    public Vector2 SnapToCardinal(Vector2 vector)
    {
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
        {
            return (vector.x > 0) ? Vector2.right : Vector2.left;
        }
        else
        {
            return (vector.y > 0) ? Vector2.up : Vector2.down;
        }
    }

    public void Dead()
    {
        if (dropItem != null)
        {
            int dropCount = dropItem.isStackable ? 1 : 0;
            SpawnItemManager.instance.SpawnItem(transform.position, dropItem, dropCount);
        }
        
        Destroy(gameObject);
    }

    public void CalculateDamage(ref float damage)
    {
        
    }

    public void ApplyDamage(float damage)
    {
        TakeDamage(damage);
    }

    public void CheckState()
    {
        
    }
}
