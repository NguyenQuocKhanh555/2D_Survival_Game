using UnityEngine;

public class MobPatrolState : MobState
{
    private Vector2[] _directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private Vector2 _chosenDirection;
    private Vector2 _targetPosition;
    private float _patrolDistance;

    public MobPatrolState(Mob mob) : base(mob) { }

    public override void Enter()
    {
        _chosenDirection = _directions[Random.Range(0, _directions.Length)];
        mob.lastMotionVector = _chosenDirection;
        _patrolDistance = Random.Range(2f, 3f);
        _targetPosition = (Vector2)mob.transform.position + _chosenDirection * _patrolDistance;
        mob.rb.linearVelocity = _chosenDirection * mob.mobPatrolSpeed;
        mob.animator.SetFloat("lastHorizontal", _chosenDirection.x);
        mob.animator.SetFloat("lastVertical", _chosenDirection.y);
        mob.animator.SetBool("isPatrol", true);
    }

    public override void Update()
    {
        if (mob.mobType == MobType.MeleeEnemy && mob.meleeAttack.CanAttack())
        {
            mob.ChangeState(new EnemyMeleeAttackState(mob));
            return;
        }

        if (mob.mobType == MobType.MeleeEnemy && mob.detector.isPlayerInDectectRange && !mob.meleeAttack.IsPlayerInAttackRange)
        {
            mob.ChangeState(new EnemyChaseState(mob));
            return;
        }

        if (mob.mobType == MobType.Animal && mob.isHit)
        {
            mob.ChangeState(new AnimalRunState(mob));
            return;
        }

        if (mob.teleport != null && mob.teleport.CanTeleport())
        {
            mob.ChangeState(new ShadowMageTeleportState(mob));
            return;
        }

        if (mob.mobType == MobType.RangeEnemy && mob.rangeAttack.CanAttack())
        {
            mob.ChangeState(new EnemyRangeAttackState(mob));
            return;
        }

        if (Vector2.Distance(mob.transform.position, _targetPosition) < 0.1f)
        {
            mob.ChangeState(new MobIdleState(mob));
            return;
        }
    }

    public override void Exit()
    {
        mob.animator.SetBool("isPatrol", false);
        mob.rb.linearVelocity = Vector2.zero;
    }
}
