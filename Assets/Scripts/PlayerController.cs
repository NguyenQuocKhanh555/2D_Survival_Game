using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _runSpeed = 1.0f;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Animator animator;
    private Player player;
    private PlayerInteractController interactController;

    private Vector2 _moveInput;
    private Vector2 _lastMotionVector;
    private bool _isRunning;
    private bool _isMoving;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        interactController = GetComponent<PlayerInteractController>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.PlayerMovement.Interact.performed += OnInteract;
        playerControls.PlayerMovement.Action.performed += OnAction;
    }

    private void OnDisable()
    {
        playerControls.PlayerMovement.Interact.performed -= OnInteract;
        playerControls.PlayerMovement.Action.performed -= OnAction;
        playerControls.Disable();
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
        _moveInput = playerControls.PlayerMovement.Move.ReadValue<Vector2>();
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        _isRunning = isShiftPressed;

        animator.SetFloat("horizontal", _moveInput.x);
        animator.SetFloat("vertical", _moveInput.y);

        _isMoving = _moveInput.x != 0 || _moveInput.y != 0;
        animator.SetBool("moving", _isMoving);

        if (_moveInput.x != 0 || _moveInput.y != 0)
        {
            _lastMotionVector = _moveInput.normalized;

            animator.SetFloat("lastHorizontal", _lastMotionVector.x);
            animator.SetFloat("lastVertical", _lastMotionVector.y);
        }
    }

    private void Move()
    {
        float currentSpeed = _isRunning ? _runSpeed : _walkSpeed;
        Vector2 movement = _moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactController.Interact(player);
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        animator.SetTrigger("action");
    }
}
