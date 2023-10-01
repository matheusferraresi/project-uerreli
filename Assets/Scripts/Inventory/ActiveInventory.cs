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
        
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int slotIndexNum)
    {
        ToggleActiveHighlight(slotIndexNum - 1);
    }

    private void ToggleActiveHighlight(int slotIndexNum)
    {
        activeSlotIndexNum = slotIndexNum;

        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        transform.GetChild(slotIndexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        // If there is a weapon in the active slot, destroy it
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // If there is no weapon in the active slot, remove the weapon
        if (!transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
        {
            ActiveWeapon.Instance.RemoveWeapon();
            return;
        }
        
        // Get the weapon prefab from the active slot
        // transform.GetChild(activeSlotIndexNum)
        // Get the weapon info from the weapon prefab
        // GetComponent<InventorySlot>().GetWeaponInfo()._weaponPrefab;
        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>()
            .GetWeaponInfo()._weaponPrefab;

        // Spawn the weapon prefab
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position,
            ActiveWeapon.Instance.transform.rotation);
        
        // Zero out the rotation of the active weapon
        // ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}