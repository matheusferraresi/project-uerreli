using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparencyDetection : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer _spriteRenderer;
    private Tilemap _tilemap;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this object collides with player
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
            {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, transparencyAmount));
            } else if (_tilemap)
            {
                StartCoroutine(FadeRoutine(_tilemap, fadeTime, _tilemap.color.a, transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (_spriteRenderer)
            {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, 1f));
            } else if (_tilemap)
            {
                StartCoroutine(FadeRoutine(_tilemap, fadeTime, _tilemap.color.a, 1f));
            }
        }
        
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetValue)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            
            float newAlpha = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeTime);

            // need to pass in each color before updating alpha value
            // there is no way to interact with the alpha value directly
            Color spriteColor = spriteRenderer.color;
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b,
                newAlpha);
            
            yield return null;
        }
    }

    // Overwriting function to work with tilemaps
    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetValue)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            
            float newAlpha = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeTime);

            // need to pass in each color before updating alpha value
            // there is no way to interact with the alpha value directly
            Color spriteColor = tilemap.color;
            tilemap.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b,
                newAlpha);
            
            yield return null;
        }
    }
}