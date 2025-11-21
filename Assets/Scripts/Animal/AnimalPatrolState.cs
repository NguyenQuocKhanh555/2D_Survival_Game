using UnityEngine;

public class AnimalPatrolState : AnimalState
{
    private Vector2[] _directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private Vector2 _chosenDirection;
    private Vector2 _targetPosition;
    private float _patrolDistance;

    public AnimalPatrolState(Animal animal) : base(animal) { }

    public override void Enter()
    {
        _chosenDirection = _directions[Random.Range(0, _directions.Length)];
        animal.lastMotionVector = _chosenDirection;
        _patrolDistance = Random.Range(2f, 3f);
        _targetPosition = (Vector2)animal.transform.position + _chosenDirection * _patrolDistance;

        animal.animator.SetFloat("lastHorizontal", _chosenDirection.x);
        animal.animator.SetFloat("lastVertical", _chosenDirection.y);
        animal.animator.SetBool("isPatrol", true);
    }

    public override void Update()
    {
        animal.rb.linearVelocity = _chosenDirection * animal.patrolSpeed;

        if (Vector2.Distance(animal.transform.position, _targetPosition) < 0.1f)
        {
            animal.ChangeState(new AnimalIdleState(animal));
            return;
        }
    }

    public override void Exit()
    {
        animal.animator.SetBool("isPatrol", false);
    }
}
