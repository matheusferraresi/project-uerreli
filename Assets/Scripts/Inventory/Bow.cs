using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowSpawnPoint;

    private Animator _animator;
    private static readonly int Fire = Animator.StringToHash("Fire");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Attack()
    {
        GameObject newArrow =
            Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, ActiveWeapon.Instance.transform.rotation);
        
        _animator.SetTrigger(Fire);
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}