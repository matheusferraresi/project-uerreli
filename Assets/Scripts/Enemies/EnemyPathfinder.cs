using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPathfinder : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    [SerializeField] private float roamingChangeTimer = 2f;

    private State _state;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _state = State.Roaming;
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (_state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            _enemyAI.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamingChangeTimer);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
