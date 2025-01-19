using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20; // Урон, который наносит пуля

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered on collision");
        // Проверяем, есть ли у объекта компонент EnemyHealth
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // Наносим урон врагу
            Destroy(gameObject); // Уничтожаем пулю
        }
    }
}
