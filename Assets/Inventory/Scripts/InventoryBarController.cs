using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryBarController : MonoBehaviour
{

    [SerializeField] private RectTransform _selectedSlot;

    [SerializeField] private ItemSlot[] _itemSlots;

    private int _selectedIndex = 0;

    private InventoryControls _controls;
    private InputAction _scroll;
    private Vector2 _scroll2;

    private float _slotWidth;

    private void Awake()
    {
        _controls = new InventoryControls();
    }

    void Start()
    {
        _slotWidth = _selectedSlot.rect.width;
        Debug.Log(_slotWidth);
    }

    private void OnEnable()
    {
        _scroll = _controls.Inventory.MouseScroll;
        _controls.Inventory.Enable();
    }

    private void OnDisable()
    {
        _controls.Inventory.Disable();
    }

    void Update()
    {
        ScrollSelectedSlot();
    }

    

    private void ScrollSelectedSlot()
    {
        _scroll2 = _scroll.ReadValue<Vector2>().normalized;
        if(_scroll2 != Vector2.zero)
        {
            if (_scroll2.y > 0)
                _selectedIndex = (_selectedIndex + 1) % _itemSlots.Length;
            else
                _selectedIndex = (_selectedIndex - 1 + _itemSlots.Length) % _itemSlots.Length;

            float y = _selectedSlot.anchoredPosition.y;
            float x = _slotWidth / 2f + _selectedIndex * _slotWidth;
            _selectedSlot.anchoredPosition = new Vector2(x, y);
            Debug.Log(_selectedIndex);
        }
            

    }

}
