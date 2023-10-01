using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator _animator;
    private static readonly int StaffAttack = Animator.StringToHash("Attack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        _animator.SetTrigger(StaffAttack);
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void SpawnStaffProjectileAnimationEvent()
    {
        GameObject magicLaserInstance = Instantiate(magicLaser, magicLaserSpawnPoint.position, magicLaserSpawnPoint.rotation);
        magicLaserInstance.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }
    
    private void MouseFollowWithOffset()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if (mousePosition.x < playerPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
