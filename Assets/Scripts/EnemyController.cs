using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private Transform _player;       
    private float _lastAttackTime = 0f;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        if(!_player) Debug.LogError("Player is null");
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
        transform.position = Vector2.MoveTowards(transform.position, _player.position, (moveSpeed + Random.Range(-0.2f,0.2f)) * Time.deltaTime);
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
