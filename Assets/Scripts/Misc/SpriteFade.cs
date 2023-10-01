using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private float fadeTime = .4f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator SlowFadeRoutine()
    {
        float elapsedTime = 0;
        float startValue = _spriteRenderer.color.a;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);

            // need to pass in each color before updating alpha value
            // there is no way to interact with the alpha value directly
            Color spriteColor = _spriteRenderer.color;
            _spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b,
                newAlpha);
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
