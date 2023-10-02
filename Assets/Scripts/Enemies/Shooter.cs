using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _projectilePrefab;

    public void Attack()
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.right = targetDirection;
    }
}
