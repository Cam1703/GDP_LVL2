using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput _playerInput;
    public static Vector2 movement;
    public static bool jump;
    public static bool interact;


    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _jumpAction;

    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;
    private InputAction _interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];

        //_menuOpenAction = _playerInput.actions["MenuOPEN"];
        //_menuCloseAction = _playerInput.actions["MenuCLOSE"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        jump = _jumpAction.IsPressed();
        interact = _interactAction.IsPressed();

        //attack = _attackAction.WasPerformedThisFrame();

        //menuOpenInput = _menuOpenAction.WasPerformedThisFrame();
        //menuCloseInput = _menuCloseAction.WasPerformedThisFrame();
    }
}
