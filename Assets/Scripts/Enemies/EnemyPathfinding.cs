using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D _rigidbody;
    private Vector2 _movePosition;
    private Knockback _knockback;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        if (_knockback.GettingKnockedBack) { return; }
        
        // Multiplying floats first here means you multiply the Vector only once and improves speed
        Vector2 newPosition = _rigidbody.position + _movePosition * (moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);

        _spriteRenderer.flipX = _movePosition.x < 0;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        _movePosition = targetPosition;
    }

    public void StopMoving()
    {
        _movePosition = Vector3.zero;
    }
}
