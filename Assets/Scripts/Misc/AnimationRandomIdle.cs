using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationRandomIdle : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (_animator)
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            _animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        }
    }
}
