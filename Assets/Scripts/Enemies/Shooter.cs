using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;

    private bool _isShooting = false;

    public void Attack()
    {
        if (!_isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;
        
        float startAngle, currentAngle, angleStep;
        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);

        for (int i = 0;
             i < burstCount;
             i++)
        {
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newProjectile = Instantiate(projectilePrefab, pos, Quaternion.identity);
                newProjectile.transform.right = newProjectile.transform.position - transform.position;

                if (newProjectile.TryGetComponent(out Projectile projectile))
                {
                    projectile.SetProjectileSpeed(projectileMoveSpeed);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);
        }

        yield return new WaitForSeconds(restTime);
        _isShooting = false;
    }

    private float TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        
        float endAngle = targetAngle;
        currentAngle = targetAngle;
        
        float halfAngleSpread = 0f;
        angleStep = 0;

        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }

        return startAngle;
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}