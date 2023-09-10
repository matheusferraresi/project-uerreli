using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private float swordCooldown = .5f;

    private Transform _weaponCollider;
    private Animator _animator;
    private GameObject _slashAnimation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _weaponCollider = PlayerController.Instance.GetWeaponCollider();
        
        // Just for learning purposes - this is not the best way to do this
        slashAnimationSpawnPoint = GameObject.Find("SlashAnimationSpawnPoint").transform;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        // _isAttacking = true;

        _animator.SetTrigger("Attack");
        _weaponCollider.gameObject.SetActive(true);

        _slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, quaternion.identity);
        _slashAnimation.transform.parent = this.transform.parent;

        StartCoroutine(AttackCooldownRoutine());
    }

    public void AnimationDoneAttacking()
    {
        _weaponCollider.gameObject.SetActive(false);
    }

    public void AnimationSwingUpFlipped()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void AnimationSwingDownFlipped()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if (mousePosition.x < playerPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            _weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(swordCooldown);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}