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
        if (mob.CanAttack() && mob.mobType == MobType.RangeEnemy)
        {
            mob.ChangeState(new EnemyRangeAttackState(mob));
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
