using UnityEngine;

public class EnemyChaseState : MobState
{
    public EnemyChaseState(Mob mob) : base(mob)
    {
    }
    
    public override void Enter()
    {
        mob.animator.SetBool("isChase", true);
    }

    public override void Update()
    {
        if (mob.mobType == MobType.MeleeEnemy && mob.meleeAttack.CanAttack())
        {
            mob.ChangeState(new EnemyMeleeAttackState(mob));
            return;
        }

        if (!mob.detector.isPlayerInDectectRange)
        {
            mob.animator.SetBool("isChase", false);
            mob.ChangeState(new MobIdleState(mob));
            return;
        }

        Vector2 direction = mob.detector.MobToPlayer;
        Vector2 faceDirection = mob.SnapToCardinal(direction);
        mob.rb.linearVelocity = direction * mob.mobRunSpeed;
        mob.lastMotionVector = faceDirection;
        mob.animator.SetFloat("lastHorizontal", faceDirection.x);
        mob.animator.SetFloat("lastVertical", faceDirection.y);
    }

    public override void Exit()
    {

    }
}

