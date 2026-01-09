using UnityEngine;

public class EnemyMeleeAttackState : MobState
{
    public EnemyMeleeAttackState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        mob.rb.linearVelocity = Vector2.zero;

        Vector2 direction = mob.detector.MobToPlayer;
        Vector2 faceDirection = mob.SnapToCardinal(direction);
        mob.lastMotionVector = faceDirection;
        mob.animator.SetFloat("lastHorizontal", faceDirection.x);
        mob.animator.SetFloat("lastVertical", faceDirection.y);

        mob.meleeAttack.MeleeAttack(faceDirection);
        mob.animator.SetTrigger("attack");
    }

    public override void Update()
    {
        if (!mob.meleeAttack.isAttacking)
        {
            mob.animator.SetBool("isChase", false);
            mob.ChangeState(new MobIdleState(mob));
            return;
        }
    }

    public override void Exit()
    {
    }
}
