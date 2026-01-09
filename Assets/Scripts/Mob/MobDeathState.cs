using UnityEngine;

public class MobDeathState : MobState
{
    public MobDeathState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        mob.rb.linearVelocity = Vector2.zero;
        mob.isDead = false;
        mob.animator.SetTrigger("die");
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
