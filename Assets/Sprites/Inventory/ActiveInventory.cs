using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void Start()
    {
        _playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }
    
    private void ToggleActiveSlot(int slotIndexNum)
    {
        ToggleActiveHighlight(slotIndexNum);
    }
    
    private void ToggleActiveHighlight(int slotIndexNum)
    {
        activeSlotIndexNum = slotIndexNum;

        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        
        transform.GetChild(slotIndexNum - 1).GetChild(0).gameObject.SetActive(true);
    }
}