using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Animator _animator;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
    }
}
