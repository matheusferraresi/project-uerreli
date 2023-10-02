using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 2f;

    private bool _isGrowing = true;
    private float _laserRange;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _capsuleCollider2D;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        {
            _isGrowing = false;
        }
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;

        transform.right = -direction;
    }
    
    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (_spriteRenderer.size.x < _laserRange && _isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / laserSpeed;
            
            // Sprite
            _spriteRenderer.size = new Vector2(Mathf.Lerp(1f, _laserRange, linearTime), 1f);
            
            // Collider
            _capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, _laserRange, linearTime), _capsuleCollider2D.size.y);
            _capsuleCollider2D.offset = new Vector2(Mathf.Lerp(1f, _laserRange, linearTime) / 2, _capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
    
    public void UpdateLaserRange(float laserRange)
    {
        _laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }
}
