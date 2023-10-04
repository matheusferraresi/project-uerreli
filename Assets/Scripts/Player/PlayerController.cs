using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private TrailRenderer playerTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCooldown = .25f;

    private PlayerControls _playerControls;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Knockback _knockBack;

    private float _baseMoveSpeed;
    private bool _isDashing;

    private bool _facingLeft;
    public bool FacingLeft => _facingLeft;

    protected override void Awake()
    {
        base.Awake();

        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _knockBack = GetComponent<Knockback>();
        _baseMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void Start()
    {
        _playerControls.Combat.Dash.performed += _ => Dash();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        _movement = _playerControls.Movement.Move.ReadValue<Vector2>();
        _animator.SetFloat("moveX", _movement.x);
        _animator.SetFloat("moveY", _movement.y);
    }

    private void Move()
    {
        if (_knockBack.GettingKnockedBack) return;

        // Multiplying floats first here means you multiply the Vector only once and improves speed
        Vector2 newPosition = _rb.position + _movement * (moveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(newPosition);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePosition.x < playerPosition.x)
        {
            _spriteRenderer.flipX = true;
            _facingLeft = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
            _facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!_isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            _isDashing = true;
            moveSpeed *= dashSpeed;
            playerTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed = _baseMoveSpeed;
        playerTrailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        _isDashing = false;
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
}