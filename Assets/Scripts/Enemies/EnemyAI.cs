using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D _rigidbody;
    private Vector2 _movePosition;
    private Knockback _knockback;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }
    
    private void FixedUpdate()
    {
        if (_knockback.GettingKnockedBack) { return; }
        
        // Multiplying floats first here means you multiply the Vector only once and improves speed
        Vector2 newPosition = _rigidbody.position + _movePosition * (moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);
    }

    public void MoveTo(Vector2 targetPosition)
    {
        _movePosition = targetPosition;
    }
}
