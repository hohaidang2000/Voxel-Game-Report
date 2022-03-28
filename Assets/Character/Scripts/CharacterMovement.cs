
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    #region Variables

    private CharacterController _controller;
    private CharacterControls _controls;

    [Header("Move")]

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;

    private InputAction _move;
    private Vector2 _move2;
    private Vector3 _move3;
    private Vector3 _moveDirection;
    private float _speed;


    [Header("Jump")]

    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _offset;
    [SerializeField] private LayerMask _groundMask;

    private bool isJump;
    private Vector3 velocity;

    [Header("Look")]

    [SerializeField] private Transform _head;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _sensitivity;

    private InputAction _look;
    private Vector2 _look2;
    private float _xRotation;


    [Header("Attack")]

    [SerializeField] private Transform _hand;
    [SerializeField] private float _attackRange;
    private bool _isAttack;


    // Animator

    private Animator _animator;

    private int horizHash;
    private int vertHash;
    private int movingHash;
    private int runningHash;
    private int jumpHash;
    private int speedHash;
    private int attackHash;

    private bool idle = true;
    private bool moving = false;
    private bool running = false;
    private bool jump = false;


    #endregion


    #region MonoBehavior

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        horizHash = Animator.StringToHash("horizontal");
        vertHash = Animator.StringToHash("vertical");
        movingHash = Animator.StringToHash("moving");
        runningHash = Animator.StringToHash("running");
        jumpHash = Animator.StringToHash("jump");
        speedHash = Animator.StringToHash("speed");
        attackHash = Animator.StringToHash("isAttack");

        _speed = _walkSpeed;

    }

    private void Awake()
    {
        _controls = new CharacterControls();

        _controls.Character.Sprint.performed += ctx => _speed = _sprintSpeed;
        _controls.Character.Sprint.canceled += ctx => _speed = _walkSpeed;

        _controls.Character.Attack.performed += ctx => _isAttack = true;
        _controls.Character.Attack.canceled += ctx => _isAttack = false;

    }

    private void OnEnable()
    {
        _move = _controls.Character.Move;
        _look = _controls.Character.Look;
        _controls.Character.Jump.started += PressSpace;
        _controls.Character.Enable();
    }



    private void OnDisable()
    {
        _controls.Character.Jump.started -= PressSpace;
        _controls.Character.Disable();
    }

    private void Update()
    {
        InputsHandler();
        Look();
        Interact();

    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
        Jump();
        AnimatorController();
    }

    #endregion


    #region Methods

    private void InputsHandler()
    {
        _move2 = _move.ReadValue<Vector2>();
        _move3 = new Vector3(_move2.x, 0f, _move2.y);

        _moveDirection = transform.forward * _move3.z + transform.right * _move3.x;
        _moveDirection.y = 0f;


    }

    private void Move()
    {

        _controller.Move(_moveDirection * _speed * Time.deltaTime);

    }

    private void PressSpace(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
            isJump = true;
    }

    private void Jump()
    {
        if (isJump)
        {
            velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);

            isJump = false;
        }
    }

    private void Gravity()
    {
        velocity.y += _gravityValue * Time.deltaTime;

        if (IsGrounded() && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        _controller.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - _offset, transform.position.z);
        if (Physics.CheckSphere(spherePos, _controller.radius - 0.05f, _groundMask))
            return true;
        return false;
    }

    private void Look()
    {
        _look2 = _look.ReadValue<Vector2>();

        float mouseX = _look2.x * _sensitivity * Time.deltaTime;
        float mouseY = _look2.y * _sensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _head.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void Interact()
    {
        if (!_isAttack)
            return;

        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _attackRange))
        {
            Debug.Log(hit.collider.name);
            Destroy(hit.collider.gameObject);
        }

        //Debug.Log(_hand.transform.childCount);
    }

    private void AnimatorController()
    {
        if (_move2 != Vector2.zero)
        {
            _animator.SetFloat(horizHash, _move2.x);
            _animator.SetFloat(vertHash, _move2.y);
        }

        moving = (_moveDirection != Vector3.zero);
        _animator.SetBool(movingHash, moving);

        //animator.SetBool(runningHash, running);
        //if (running)
        //{
        //    animator.SetFloat(speedHash, _runSpeed + 1f);
        //}

        _animator.SetBool(jumpHash, jump);

        if (_isAttack && !_animator.GetBool(attackHash))
            _animator.SetBool(attackHash, true);
        else if (!_isAttack && _animator.GetBool(attackHash))
            _animator.SetBool(attackHash, false);

    }



    #endregion
}
