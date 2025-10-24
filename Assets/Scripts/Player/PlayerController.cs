using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _runSpeed = 1.0f;
    [SerializeField] private float _staminaConsumptionRate = 10.0f;

    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Player _player;
    private PlayerInteractController _interactController;
    private PlayerUIInteractController _inventoryController;
    private PlayerToolbarController _toolbarController;

    private Vector2 _moveInput;
    private Vector2 _lastMotionVector;
    private float _scrollInput;
    private bool _isRunning;
    private bool _isMoving;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _interactController = GetComponent<PlayerInteractController>();
        _inventoryController = GetComponent<PlayerUIInteractController>();
        _toolbarController = GetComponent<PlayerToolbarController>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.PlayerMovement.Interact.performed += OnInteract;
        _playerControls.PlayerMovement.Action.performed += OnAction;
        _playerControls.UI.OpenInventory.performed += OnOpenInventory;
    }

    private void OnDisable()
    {
        _playerControls.UI.OpenInventory.performed -= OnOpenInventory;
        _playerControls.PlayerMovement.Interact.performed -= OnInteract;
        _playerControls.PlayerMovement.Action.performed -= OnAction;
        _playerControls.Disable();
    }

    private void Update()
    {
        PlayerMovementInput();
        PlayerToolbarSelectInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerMovementInput()
    {
        _moveInput = _playerControls.PlayerMovement.Move.ReadValue<Vector2>();
        _isRunning = !_player.isExhausted &&
            (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed);

        _animator.SetFloat("horizontal", _moveInput.x);
        _animator.SetFloat("vertical", _moveInput.y);

        _isMoving = _moveInput.x != 0 || _moveInput.y != 0;
        _animator.SetBool("moving", _isMoving);

        if (_moveInput.x != 0 || _moveInput.y != 0)
        {
            _lastMotionVector = _moveInput.normalized;

            _animator.SetFloat("lastHorizontal", _lastMotionVector.x);
            _animator.SetFloat("lastVertical", _lastMotionVector.y);
        }
    }

    private void PlayerToolbarSelectInput()
    {
        _scrollInput = _playerControls.UI.SelectOnToolbar.ReadValue<float>();
        if (_scrollInput != 0)
            _toolbarController.SelectToolbarIndex(_scrollInput);
    }

    private void Move()
    {
        float currentSpeed = _isRunning ? _runSpeed : _walkSpeed;
        if (_isRunning) {
            _player.UseStamina(_staminaConsumptionRate * Time.fixedDeltaTime);
        }
        Vector2 movement = _moveInput * currentSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movement);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        _interactController.Interact(_player);
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("action");
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        _inventoryController.ToggleInventory();
        if (_inventoryController.IsInventoryOpen)
            _playerControls.PlayerMovement.Disable();
        else
            _playerControls.PlayerMovement.Enable();
    }
}
