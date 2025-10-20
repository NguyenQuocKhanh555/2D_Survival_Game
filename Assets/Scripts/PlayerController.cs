using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 1.0f;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool isRunning;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        moveInput = playerControls.PlayerMovement.Move.ReadValue<Vector2>();
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        isRunning = isShiftPressed;

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetFloat("lastHorizontal", moveInput.x);
            animator.SetFloat("lastVertical", moveInput.y);
        }
    }

    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector2 movement = moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
