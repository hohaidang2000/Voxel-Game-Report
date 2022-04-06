using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    private CharacterController _controller;

    [Header("Move")]

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    
    [Header("Jump")]

    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _offset;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 velocity;

    #endregion


    #region MonoBehavior

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

    }

    private void Update()
    {
   
    }

    #endregion


    #region Methods

    public Vector3 GetMoveDirection(Vector2 moveInput)
    {
        Vector3 move3 = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 moveDirection = transform.forward * move3.z + transform.right * move3.x;
        moveDirection.y = 0f;

        return moveDirection;

    }

    public void Move(Vector3 moveInput, bool isSprinting)
    {
        Vector3 moveDirection = GetMoveDirection(moveInput);
        float speed = isSprinting ? _sprintSpeed : _walkSpeed;
        _controller.Move(moveDirection * speed * Time.deltaTime);

    }

    public void Gravity()
    {
        velocity.y += _gravityValue * Time.deltaTime;

        if (IsGrounded() && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        _controller.Move(velocity * Time.deltaTime);
    }


    public void Jump(bool isJumping)
    {
        if (isJumping && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
        }
    }

    private bool IsGrounded()
    {
        _offset = -(_controller.radius - 0.1f); 
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - _offset, transform.position.z);
        if (Physics.CheckSphere(spherePos, _controller.radius, _groundMask))
            return true;
        return false;
    }

    #endregion
}
