using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject _weaponPrefab;
    public float _weaponCooldown;
}
