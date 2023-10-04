using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f; 
    [SerializeField] private float damageRecoveryTime = 1f;
    
    private Knockback _knockBack;
    private Flash _flash;
    
    private int _currentHealth;
    private bool _canTakeDamage = true;

    protected override void Awake()
    {
        base.Awake();
        _flash = GetComponent<Flash>();
        _knockBack = GetComponent<Knockback>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyPathfinding enemyPathfinding = other.collider.GetComponent<EnemyPathfinding>();

        if (enemyPathfinding)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!_canTakeDamage) return;
        
        ScreenShakeManager.Instance.ShakeScreen();
        
        _knockBack.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(_flash.FlashRoutine());
        _canTakeDamage = false;
        _currentHealth -= damageAmount;
        StartCoroutine(DamageRecoverynRoutine());
    }
    
    public void HealPlayer(int healAmount = 1)
    {
        _currentHealth += healAmount;
    }

    private IEnumerator DamageRecoverynRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }
}
