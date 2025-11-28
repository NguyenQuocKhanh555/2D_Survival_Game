using UnityEngine;

public class ShadowMageTeleportState : MobState
{
    public ShadowMageTeleportState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        mob.animator.SetTrigger("teleport");
        mob.teleport.CreateOpenPort();
    }

    public override void Update()
    {
        if (mob.rangeAttack.CanAttack())
        {
            mob.ChangeState(new EnemyRangeAttackState(mob));
            return;
        }

        if (!mob.rangeAttack.CanAttack())
        {
            mob.ChangeState(new MobIdleState(mob));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
