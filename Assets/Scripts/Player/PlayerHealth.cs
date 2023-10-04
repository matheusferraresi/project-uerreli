using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f; 
    [SerializeField] private float damageRecoveryTime = 1f;
    
    private Knockback _knockBack;
    private Flash _flash;
    private Slider _healthSlider;
    
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
        UpdateHealthSlider();
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
        UpdateHealthSlider();
        CheckPlayerDeath();
    }
    
    public void CheckPlayerDeath()
    {
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            // GameManager.Instance.GameOver();
            Debug.Log("Player is dead");
        }
    }
    
    public void HealPlayer(int healAmount = 1)
    {
        if (_currentHealth < maxHealth)
        {
            _currentHealth += healAmount;
            UpdateHealthSlider();
        }
    }

    private IEnumerator DamageRecoverynRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (_healthSlider == null)
        {
            _healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = _currentHealth;
    }
}
