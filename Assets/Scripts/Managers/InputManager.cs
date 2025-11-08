using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput _playerInput;
    public static Vector2 movement;
    public static bool sprint;
    public static bool jump;
    public static bool jumpEnter;
    public static bool interact;
    public static bool inventoryOnFlag;
    public static bool pauseOnFlag;
    public static bool inventoryOffFlag;
    public static bool pauseOffFlag;
    public static bool attack;
    public static Vector2 navigation;

    private InputAction _moveAction;
    private InputAction _sprintAction;
    private InputAction _attackAction;
    private InputAction _jumpAction;

    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;
    private InputAction _interactAction;

    private InputAction _inventoryOnControl;
    private InputAction _pauseOnControl;
    private InputAction _inventoryOffControl;
    private InputAction _pauseOffControl;
    private InputAction _navigationControl;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _sprintAction = _playerInput.actions["Sprint"];
        _attackAction = _playerInput.actions["Attack"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];

        _inventoryOnControl = _playerInput.actions["Inventory"];
        _pauseOnControl = _playerInput.actions["Pause"];

        _playerInput.SwitchCurrentActionMap("UI");

        _inventoryOffControl = _playerInput.actions["Inventory"];
        _pauseOffControl = _playerInput.actions["Pause"];
        _navigationControl = _playerInput.actions["Navigate"];

        _playerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        sprint = _sprintAction.IsPressed();
        jump = _jumpAction.IsPressed();
        jumpEnter = _jumpAction.WasPressedThisFrame();

        interact = _interactAction.IsPressed();
        attack = _attackAction.IsPressed();

        inventoryOnFlag = _inventoryOnControl.WasPressedThisFrame();
        pauseOnFlag = _pauseOnControl.WasPressedThisFrame();
        inventoryOffFlag = _inventoryOffControl.WasPressedThisFrame();
        pauseOffFlag = _pauseOffControl.WasPressedThisFrame();
        navigation = _navigationControl.ReadValue<Vector2>();
    }
}
