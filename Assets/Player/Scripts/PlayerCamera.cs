using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private float _sensitivity;
    [SerializeField] private PlayerInput _playerInput;

    private float _xRotation;

    private void Awake()
    {
        _playerInput = _player.GetComponent<PlayerInput>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        float mouseX = _playerInput.MouseLook.x * _sensitivity * Time.deltaTime;
        float mouseY = _playerInput.MouseLook.y * _sensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _player.Rotate(Vector3.up * mouseX);
    }
}
