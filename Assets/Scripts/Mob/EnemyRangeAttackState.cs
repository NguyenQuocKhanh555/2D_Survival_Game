using UnityEngine;

public class EnemyRangeAttackState : MobState
{
    public EnemyRangeAttackState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {        
        Vector2 direction = mob.detector.MobToPlayer;
        Vector2 projectileSpawnPoint = (Vector2)mob.transform.position + direction;
        Vector2 faceDirection = mob.SnapToCardinal(direction);
        mob.lastMotionVector = faceDirection;
        mob.animator.SetFloat("lastHorizontal", faceDirection.x);
        mob.animator.SetFloat("lastVertical", faceDirection.y);
        
        mob.rangeAttack.SetUp(projectileSpawnPoint, direction);        
        mob.animator.SetTrigger("attack");
    }

    public override void Update()
    {
        if (mob.teleport != null && mob.teleport.CanTeleport())
        {
            mob.ChangeState(new ShadowMageTeleportState(mob));
            return;
        }

        if (!mob.rangeAttack.isAttacking)
        {
            mob.ChangeState(new MobIdleState(mob));
            return;
        }    
    }

    public override void Exit()
    {
        
    }
}
