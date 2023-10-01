using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 22f;
    [SerializeField] private GameObject onHitVFX;
    
    private WeaponInfo _weaponInfo;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectRange();
    }
    
    public void SetWeaponInfo(WeaponInfo weaponInfo)
    {
        _weaponInfo = weaponInfo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();

        if (!other.isTrigger && (enemyHealth || indestructible))
        {
            Instantiate(onHitVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    
    private void DetectRange()
    {
        if (Vector3.Distance(_startPosition, transform.position) > _weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * projectileSpeed);
    }
}