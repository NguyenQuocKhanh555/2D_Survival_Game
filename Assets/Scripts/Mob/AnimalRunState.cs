using UnityEngine;

public class AnimalRunState : MobState
{
    private Vector2 _targetPosition;

    public AnimalRunState(Mob mob) : base(mob)
    {
    }

    public override void Enter()
    {
        Vector2 direction = mob.detector.PlayerToMob;
        _targetPosition = (Vector2)mob.transform.position + direction * 5f;
        mob.rb.linearVelocity = direction * mob.mobRunSpeed;
        mob.animator.SetFloat("lastHorizontal", direction.x);
        mob.animator.SetFloat("lastVertical", direction.y);
        mob.animator.SetBool("isRun", true);
    }

    public override void Update()
    {
        if (Vector2.Distance(mob.transform.position, _targetPosition) < 0.1f)
        {
            mob.ChangeState(new MobIdleState(mob));
            return;
        }
    }

    public override void Exit()
    {
        mob.animator.SetBool("isRun", false);
    }
}
