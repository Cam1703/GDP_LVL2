using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput _playerInput;
    public static Vector2 movement;

    private InputAction _moveAction;
    private InputAction _attackAction;

    private InputAction _menuOpenAction;
    private InputAction _menuCloseAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];

        //_menuOpenAction = _playerInput.actions["MenuOPEN"];
        //_menuCloseAction = _playerInput.actions["MenuCLOSE"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        //attack = _attackAction.WasPerformedThisFrame();

        //menuOpenInput = _menuOpenAction.WasPerformedThisFrame();
        //menuCloseInput = _menuCloseAction.WasPerformedThisFrame();
    }
}
