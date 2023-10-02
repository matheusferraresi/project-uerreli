using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 22f;
    [SerializeField] private GameObject onHitVFX;
    [SerializeField] private bool isEnemyProjectile;
    [SerializeField] private float projectileRange = 10f;

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

    public void SetWeaponRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    private void HandleHit()
    {
        Instantiate(onHitVFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (enemyHealth || indestructible || playerHealth)
        {
            if ((playerHealth & isEnemyProjectile) || (enemyHealth & !isEnemyProjectile))
            {
                playerHealth?.TakeDamage(1, transform);
                HandleHit();
            }
            else if (indestructible)
            {
                HandleHit();
            }
        }
    }

    private void DetectRange()
    {
        if (Vector3.Distance(_startPosition, transform.position) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * projectileSpeed);
    }
}