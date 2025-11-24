using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyRangeAttackState : MobState
{
    private Vector2 _offset = new Vector2(0, 0.8f);

    public EnemyRangeAttackState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        Vector2 direction = ((mob.attackRange.playerPosition + _offset) - (Vector2)(mob.transform.position)).normalized;
        mob.lastMotionVector = direction;
        mob.animator.SetFloat("lastHorizontal", direction.x);
        mob.animator.SetFloat("lastVertical", direction.y);
        mob.animator.SetTrigger("attack");
    }

    public override void Update()
    {
        if (mob.ShadowMageCanUsingTeleport())
        {
            mob.ChangeState(new ShadowMageTeleportState(mob));
        }

        if (!mob.CanAttack() && !mob.ShadowMageCanUsingTeleport())
        {
            mob.ChangeState(new MobIdleState(mob));
            return;
        }    
    }

    public override void Exit()
    {
        
    }
}
