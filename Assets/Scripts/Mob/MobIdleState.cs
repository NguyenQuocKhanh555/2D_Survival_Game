using UnityEngine;

public class MobIdleState : MobState
{
    private float _idleDuration;

    public MobIdleState(Mob animal) : base(animal)
    {
    }

    public override void Enter()
    {
        _idleDuration = Random.Range(2f, 5f);
        mob.animator.SetFloat("lastHorizontal", mob.lastMotionVector.x);
        mob.animator.SetFloat("lastVertical", mob.lastMotionVector.y);
        mob.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
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

        if (mob.mobType == MobType.MeleeEnemy && mob.meleeAttack.CanAttack())
        {
            mob.ChangeState(new EnemyMeleeAttackState(mob));
            return;
        }

        if (mob.mobType == MobType.MeleeEnemy && mob.detector.isPlayerInDectectRange && !mob.meleeAttack.IsPlayerInAttackRange)
        {
            Debug.Log("Chasing from idle");
            mob.ChangeState(new EnemyChaseState(mob));
            return;
        }

        _idleDuration -= Time.deltaTime;

        if (_idleDuration <= 0)
        {
            mob.ChangeState(new MobPatrolState(mob));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
