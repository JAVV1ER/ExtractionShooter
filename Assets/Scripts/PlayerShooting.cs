using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LayerMask enemyLayer;
    public float autoAimRange = 10f;
    public Transform weapon;
    public float rotationSpeed = 100f;

    private Transform _targetEnemy;
    private float _nextFireTime = 0f;
    private Inventory _inventory;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        FindClosestEnemy();
        
        if (_targetEnemy != null)
        {
            Vector2 direction = (_targetEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            
            weapon.rotation = Quaternion.RotateTowards(
                weapon.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            //Плавное возвращение в исходное состояние
            Quaternion defaultRotation = Quaternion.Euler(0, 0, 0);
            weapon.rotation = Quaternion.RotateTowards(
                weapon.rotation, 
                defaultRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
        
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime && !EventSystem.current.IsPointerOverGameObject())
        {
            var bulletItem = _inventory.GetItems().OfType<BulletItem>().FirstOrDefault();
            if (!bulletItem) return;
            if(bulletItem.Count <= 0) return;
            
            Shoot();
            _inventory.UseItem(bulletItem);
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

    public void Shoot()
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
