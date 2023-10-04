using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }
    
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f; 
    [SerializeField] private float damageRecoveryTime = 1f;
    
    private Knockback _knockBack;
    private Flash _flash;
    private Slider _healthSlider;
    
    private int _currentHealth;
    private bool _canTakeDamage = true;
    
    const string HealthSlider = "Health Slider";
    const string TownScene = "Town";
    readonly int deathAnimation = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        _flash = GetComponent<Flash>();
        _knockBack = GetComponent<Knockback>();
    }

    private void Start()
    {
        IsDead = false;
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
    
    private void CheckPlayerDeath()
    {
        if (_currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            _currentHealth = 0;
            GetComponent<Animator>().SetTrigger(deathAnimation);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }
    
    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.ResetStamina();
        SceneManager.LoadScene(TownScene);
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
            _healthSlider = GameObject.Find(HealthSlider).GetComponent<Slider>();
        }

        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = _currentHealth;
    }
}
