using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float moveSpeed = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    private Transform _player;       
    private float _lastAttackTime = 0f;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (!_player) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer(); // Идём к игроку, если он в радиусе обнаружения, но ещё не в радиусе атаки
            }
            else
            {
                AttackPlayer(); // Атакуем игрока, если он в радиусе атаки
            }
        }
    }

    void MoveTowardsPlayer()
    {
        //Vector2 direction = (_player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, _player.position, moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        // Атакуем игрока, если время между атаками прошло
        if (Time.time >= _lastAttackTime + attackCooldown)
        {
            _lastAttackTime = Time.time;
            PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
