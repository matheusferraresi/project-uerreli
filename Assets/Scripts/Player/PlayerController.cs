using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    public static PlayerController Instance; 

    private PlayerControls _playerControls;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _facingLeft = false;
    public bool FacingLeft
    {
        get { return _facingLeft;  }
        set { _facingLeft = value; }
    }

    private void Awake()
    {
        Instance = this;
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
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
}
