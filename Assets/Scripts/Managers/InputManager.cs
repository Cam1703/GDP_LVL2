using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput _playerInput;
    public static Vector2 movement;
    public static bool jump;
    public static bool interact;
    public static bool inventoryFlag;
    public static bool pauseFlag;

    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _jumpAction;

    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;
    private InputAction _interactAction;

    private InputAction _inventoryControl;
    private InputAction _pauseControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];

        _inventoryControl = _playerInput.actions["Inventory"];
        _pauseControl = _playerInput.actions["Pause"];

        //_menuOpenAction = _playerInput.actions["MenuOPEN"];
        //_menuCloseAction = _playerInput.actions["MenuCLOSE"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        jump = _jumpAction.IsPressed();
        interact = _interactAction.IsPressed();
        inventoryFlag = _inventoryControl.WasPressedThisFrame();
        pauseFlag = _pauseControl.WasPressedThisFrame();
        //attack = _attackAction.WasPerformedThisFrame();

        //menuOpenInput = _menuOpenAction.WasPerformedThisFrame();
        //menuCloseInput = _menuCloseAction.WasPerformedThisFrame();
    }
}
