using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float autoAimRange = 10f;
    [SerializeField] private Transform weapon;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    
    private Transform _targetEnemy;
    private float _nextFireTime = 0f;
    private Inventory _inventory;
    private Collider2D[] _hits = new Collider2D[10];

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        if(!_inventory) Debug.LogError("No Inventory found");
    }

    void Update()
    {
        FindClosestEnemy();
        RotateWeapon();
    }

    void RotateWeapon()
    {
        //Поворачиваем пушку к врагу
        if (_targetEnemy)
        {
            Vector2 direction = (_targetEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            
            weapon.rotation = Quaternion.RotateTowards(
                weapon.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
            //При повороте зеркалим ствол, чтоб смотрелось
            if (angle >= 90f || angle <= -90f)
            {
                weaponSpriteRenderer.flipY = true;
            }
            else
            {
                weaponSpriteRenderer.flipY = false;
            }
        }
        else
        {
            //Или возвращаем в исходное
            Quaternion defaultRotation = Quaternion.Euler(0, 0, 0);
            weapon.rotation = Quaternion.RotateTowards(
                weapon.rotation, 
                defaultRotation, 
                rotationSpeed * Time.deltaTime
            );
            weaponSpriteRenderer.flipY = false;
        }
    }

    void FindClosestEnemy()
    {
        // Используем NonAlloc чтобы не перегружать память (рекомендация Rider)
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, autoAimRange, _hits, enemyLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hits[i];
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        _targetEnemy = closestEnemy;
    }

    public void Shoot()
    {
        //var bulletItem = _inventory.GetItems().OfType<BulletItem>().FirstOrDefault();
        if (!_inventory.TryGetItem<BulletItem>(out var bulletItem))
        {
            Debug.LogWarning("No Item found");
            return;
        }
        if(bulletItem.Count <= 0) return;
        
        _inventory.UseItem(bulletItem);
        _nextFireTime = Time.time + fireRate;
        
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
