using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _dodgeSpeed = 1.0f;
    [SerializeField] private float _dodgeDistance = 10f;
    [SerializeField] private float _dodgeCooldown = 1.0f;
    [SerializeField] private float _staminaDodgeConsumption = 10.0f;

    private PlayerControls _playerControls;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Player _player;
    private PlayerInteractController _interactController;
    private PlayerUIInteractController _inventoryController;
    private PlayerToolbarController _toolbarController;
    private PlayerPickupItemController _pickupItemController;
    private PlayerUseItemController _useToolController; 

    private Vector2 _moveInput;
    private Vector2 _lastMotionVector;
    private float _scrollInput;
    private float _pickupAroundInput;
    private bool _isInputMoving;
    private bool _isPointerOverUI;

    private bool _isDodging = false;
    private bool _canDodge = true;
    
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _interactController = GetComponent<PlayerInteractController>();
        _inventoryController = GetComponent<PlayerUIInteractController>();
        _toolbarController = GetComponent<PlayerToolbarController>();
        _pickupItemController = GetComponent<PlayerPickupItemController>();
        _useToolController = GetComponent<PlayerUseItemController>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.PlayerMovement.Interact.performed += OnInteract;
        _playerControls.PlayerMovement.Action.performed += OnAction;
        _playerControls.PlayerMovement.PickupSingleItem.performed += OnPickupItem;
        _playerControls.UI.OpenInventory.performed += OnOpenInventory;
        _playerControls.PlayerMovement.Dodge.performed += OnDodge;
        _playerControls.PlayerMovement.Consume.performed += OnConsumeItem;
    }

    private void OnDisable()
    {
        _playerControls.UI.OpenInventory.performed -= OnOpenInventory;
        _playerControls.PlayerMovement.Interact.performed -= OnInteract;
        _playerControls.PlayerMovement.Action.performed -= OnAction;
        _playerControls.PlayerMovement.PickupSingleItem.performed -= OnPickupItem;
        _playerControls.PlayerMovement.Dodge.performed -= OnDodge;
        _playerControls.PlayerMovement.Consume.performed -= OnConsumeItem;
        _playerControls.Disable();
    }

    private void Update()
    {
        PlayerMovementInput();
        PlayerToolbarSelectInput();
        PlayerPickupAroundInput();
        _pickupItemController.SetStateToMoving(_animator);
        _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        _useToolController.CanSelectCheck();
        _useToolController.SelectTile();
        _useToolController.Marker();
    }

    private void FixedUpdate()
    {
        Move();
        _pickupItemController.MoveToTargetPickupItem(_walkSpeed, _animator);
    }

    private void PlayerMovementInput()
    {
        _moveInput = _playerControls.PlayerMovement.Move.ReadValue<Vector2>();

        _animator.SetFloat("horizontal", _moveInput.x);
        _animator.SetFloat("vertical", _moveInput.y);

        _isInputMoving = _moveInput.x != 0 || _moveInput.y != 0;
        _animator.SetBool("moving", _isInputMoving);

        if (_isInputMoving)
        {
            _lastMotionVector = _moveInput.normalized;
            _pickupItemController.isMovingToPickupItem = false;

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

    private void PlayerPickupAroundInput()
    {
        if (_isDodging) return;
        _pickupAroundInput = _playerControls.PlayerMovement.PickupItemAround.ReadValue<float>();
        if (_pickupAroundInput != 0)
        {
            _pickupItemController.FindNearestPickupItem();
            _pickupItemController.SetTargetPickupItem();
        }          
    }

    private void Move()
    {
        if (_isDodging) return;
        Vector2 movement = _moveInput * _walkSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movement);
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        if (_isDodging || !_canDodge) return;
        if (_moveInput == Vector2.zero) return;
        StartCoroutine(DodgeCoroutine());
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        _interactController.Interact(_player);
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        if (_isDodging) return;
        if (_isPointerOverUI) return;
        if (_toolbarController.GetToolbarSelectedItem == null) return;
        switch (_toolbarController.GetToolbarSelectedItem.itemType)
        {
            case ItemTypes.Weapon:
                _useToolController.UseWeapon(_animator, _lastMotionVector);
                break;
            case ItemTypes.Tool:
                _useToolController.UseTool(_animator, _lastMotionVector);
                break;
            default:
                break;
        }
    }

    private void OnConsumeItem(InputAction.CallbackContext context)
    {
        if (_isDodging) return;
        if (_isPointerOverUI) return;
        if (_toolbarController.GetToolbarSelectedItem == null) return;
        if (_toolbarController.GetToolbarSelectedItem.itemType != ItemTypes.Consumable) return;
        _useToolController.UseConsumableItem(_animator, _lastMotionVector, _toolbarController);
    }

    private void OnPickupItem(InputAction.CallbackContext context)
    {
        if (_isDodging) return;
        _pickupItemController.PickupSingleItem();
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (_isDodging) return;
        _inventoryController.ToggleInventory();
        if (_inventoryController.IsInventoryOpen)
            _playerControls.PlayerMovement.Disable();
        else
            _playerControls.PlayerMovement.Enable();
    }

    private IEnumerator DodgeCoroutine()
    {
        _player.UseStamina(_staminaDodgeConsumption);
        _animator.SetTrigger("dodge");
        _isDodging = true;
        _canDodge = false;

        float elapsed = 0f;
        float dodgeDuration = _dodgeDistance / _dodgeSpeed;

        while (elapsed < dodgeDuration)
        {
            _rb.linearVelocity = _moveInput * _dodgeSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _isDodging = false;
        
        yield return new WaitForSeconds(_dodgeCooldown);

        _canDodge = true;
    }
}
