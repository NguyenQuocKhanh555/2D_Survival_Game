using UnityEngine;

public class AnimalIdleState : AnimalState
{
    private float _idleDuration;

    public AnimalIdleState(Animal animal) : base(animal)
    {
    }

    public override void Enter()
    {
        _idleDuration = Random.Range(2f, 5f);

        animal.animator.SetFloat("lastHorizontal", animal.lastMotionVector.x);
        animal.animator.SetFloat("lastVertical", animal.lastMotionVector.y);

        animal.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        _idleDuration -= Time.deltaTime;

        if (_idleDuration <= 0)
        {
            animal.ChangeState(new AnimalPatrolState(animal));
        }
    }

    public override void Exit()
    {
        
    }
}
