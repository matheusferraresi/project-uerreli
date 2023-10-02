using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Attacking
    }

    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float roamingChangeTimer = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopOnAttack;

    private State _state;
    private EnemyPathfinding _enemyPathfinding;
    private Vector2 _roamPosition;
    private float _roamingTimer;
    private bool _canAttack = true;

    private void Awake()
    {
        _state = State.Roaming;
        _enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Start()
    {
        _roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (_state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            default:
                Debug.LogError("Invalid state: " + _state);
                break;
        }
    }

    private void Roaming()
    {
        _roamingTimer += Time.deltaTime;
        _enemyPathfinding.MoveTo(_roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            _state = State.Attacking;
        }
        
        if (_roamingTimer > roamingChangeTimer)
        {
            _roamingTimer = 0f;
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            _state = State.Roaming;
        }
        
        if (_canAttack && attackRange != 0)
        {
            _canAttack = false;
            (enemyType as IEnemy)?.Attack();

            if (stopOnAttack)
            {
                _enemyPathfinding.StopMoving();
            }
            else
            {
                _enemyPathfinding.MoveTo(GetRoamingPosition());
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        _roamingTimer = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}