using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    public Vector2 MoveInput { get; private set; }
    public bool JumpingPressed { get; private set; }
    public bool SprintingPressed { get; private set; }
    public Vector2 MouseLook { get; private set; }

    public event Action OnAttack;
    

    private PlayerControls _controls;
    private InputAction _move;
    private InputAction _look;

    private void Awake()
    {
        _controls = new PlayerControls();

        GetJumpInput();
        GetSprintInput();
        //GetAttackInput();

    }

    private void OnEnable()
    {
        _move = _controls.Player.Move;
        _look = _controls.Player.Look;
        _controls.Player.Attack.performed += HandleAttack;
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }


    void Update()
    {
        GetMoveInput();
        GetMouseLook();
        
    }

    private void GetMoveInput()
    {
        MoveInput = _move.ReadValue<Vector2>();
    }

    private void GetJumpInput()
    {
        _controls.Player.Jump.performed += ctx => JumpingPressed = true;
        _controls.Player.Jump.canceled += ctx => JumpingPressed = false;
    }

    private void GetSprintInput()
    {
        _controls.Player.Sprint.performed += ctx => SprintingPressed = true;
        _controls.Player.Sprint.canceled += ctx => SprintingPressed = false;
    }

    private void GetMouseLook()
    {
        MouseLook = _look.ReadValue<Vector2>();
    }

    private void HandleAttack(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke();
    }

}
