using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls _playerControls;

    protected override void Awake()
    {
        base.Awake();
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

    public void EquipStartinWeapon()
    {
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

        // Get the weapon prefab from the active slot
        // transform.GetChild(activeSlotIndexNum)
        // Get the weapon info from the weapon prefab
        // GetComponent<InventorySlot>().GetWeaponInfo()._weaponPrefab;
        // GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>()
        //     .GetWeaponInfo().weaponPrefab;

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        
        // If there is no weapon in the active slot, remove the weapon
        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.RemoveWeapon();
            return;
        }

        // Spawn the weapon prefab
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position,
            ActiveWeapon.Instance.transform.rotation);
        
        // Zero out the rotation of the active weapon
        // ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}