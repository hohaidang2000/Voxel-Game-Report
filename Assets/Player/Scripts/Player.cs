using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Variables

    [SerializeField] private Transform _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerMovement _playerMovement;

    [SerializeField] private float _attackRange = 5;
    

    [SerializeField] protected LayerMask _groundMask;

    private World _world;

    #endregion


    #region MonoBehaviour
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
        _world = FindObjectOfType<World>();
    }

    private void Start()
    {
        _playerInput.OnAttack += HandleAttack;
    }

    // Update is called once per frame
    void Update()
    {
        _playerMovement.Gravity();
        _playerMovement.Jump(_playerInput.JumpingPressed);
        _playerMovement.Move(_playerInput.MoveInput, _playerInput.SprintingPressed);
    }

    #endregion


    #region Methods

    private void HandleAttack()
    {
        Ray attackRay = new Ray(_camera.position, _camera.forward);
        RaycastHit hit;
        if (Physics.Raycast(attackRay, out hit, _attackRange, _groundMask))
        {
            ModifyTerrain(hit);
        }
    }

    private void ModifyTerrain(RaycastHit hit)
    {
        _world.SetBlock(hit, BlockType.Air);
    }

    #endregion

}
