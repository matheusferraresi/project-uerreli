using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordCooldown = .5f;

    private PlayerControls _playerControls;
    private Animator _animator;
    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private GameObject _slashAnimation;

    private bool _attackButtonDown = false;
    private bool _isAttacking = false;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
        _activeWeapon = GetComponentInParent<ActiveWeapon>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerControls.Combat.Attack.started += _ => StartAttacking();
        _playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void Attack()
    {
        if (_attackButtonDown && !_isAttacking)
        {
            _isAttacking = true;

            _animator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            _slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, quaternion.identity);
            _slashAnimation.transform.parent = this.transform.parent;
            
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    public void AnimationDoneAtacking()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void AnimationSwingUpFlipped()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (_playerController.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void AnimationSwingDownFlipped()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (_playerController.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(_playerController.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if (mousePosition.x < playerPosition.x)
        {
            _activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            _activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void StartAttacking()
    {
        _attackButtonDown = true;
    }

    private void StopAttacking()
    {
        _attackButtonDown = false;
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(swordCooldown);
        _isAttacking = false;
    }
}