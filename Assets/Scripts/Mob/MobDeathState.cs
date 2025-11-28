using UnityEngine;

public class MobDieState : MobState
{
    public MobDieState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        mob.animator.SetTrigger("die");
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
