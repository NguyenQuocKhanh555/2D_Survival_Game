using UnityEngine;

public class Animal : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public float patrolSpeed = 1.0f;
    public float runSpeed = 1.0f;
    public Vector2 lastMotionVector = Vector2.down;

    private AnimalState _currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(new AnimalIdleState(this));
    }

    private void Update()
    {
        _currentState.Update();
    }

    public void ChangeState(AnimalState state)
    {
        if (_currentState != null) 
            _currentState.Exit();

        _currentState = state;
        _currentState.Enter();
    }
}
