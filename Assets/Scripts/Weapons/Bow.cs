using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowSpawnPoint;

    private Animator _animator;
    private static readonly int BowAttack = Animator.StringToHash("Fire");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.SetTrigger(BowAttack);
        GameObject newArrow =
            Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().SetProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}