using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LayerMask enemyLayer;
    public float autoAimRange = 10f;
    public Transform weapon;

    private Transform _targetEnemy;
    private float _nextFireTime = 0f;

    void Update()
    {
        FindClosestEnemy();
        
        if (_targetEnemy != null)
        {
            Vector2 direction = (_targetEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            weapon.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }
    }

    void FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, autoAimRange, enemyLayer);

        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
                //Debug.LogWarning(hit.name + " : " + hit.transform);
            }
        }

        _targetEnemy = closestEnemy;
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = firePoint.right * bulletSpeed;
        Destroy(bullet, 2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoAimRange);
    }
}
