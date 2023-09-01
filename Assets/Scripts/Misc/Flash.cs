using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMaterial;
    [SerializeField] private float restoreDefaultMaterialTime = .2f;

    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;

    public float GetRestoreMaterialTime()
    {
        return restoreDefaultMaterialTime;
    }
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        _spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(restoreDefaultMaterialTime);

        _spriteRenderer.material = _defaultMaterial;
    }
}
