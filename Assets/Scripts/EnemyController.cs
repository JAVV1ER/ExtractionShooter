using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform legLTransform;
    [SerializeField] private Transform legRTransform;
    [SerializeField] private float animationSpeed = 0.5f; // Скорость движения ног
    [SerializeField] private float movementRange = 0.2f;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    
    
    private Transform _player;       
    private float _lastAttackTime = 0f;
    private Tween _leftLegTween;
    private Tween _rightLegTween;
    private bool _animationLegIsPlaying;

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
            if(!_animationLegIsPlaying) StartLegAnimation();
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer(); // Идём к игроку, если он в радиусе обнаружения, но ещё не в радиусе атаки
            }
            else
            {
                AttackPlayer(); // Атакуем игрока, если он в радиусе атаки
            }
        }
        else
        {
            StopLegAnimation();
        }
    }

    void MoveTowardsPlayer()
    {
        FlipSprite();
        transform.position = Vector2.MoveTowards(transform.position, _player.position, (moveSpeed + Random.Range(-0.2f,0.2f)) * Time.deltaTime);
    }
    void FlipSprite()
    {
        Vector2 direction = (_player.position - transform.position).normalized;
        bodySpriteRenderer.flipX = direction.x switch
        {
            > 0 => false,
            < 0 => true,
            _ => bodySpriteRenderer.flipX
        };
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
    
    
    void StartLegAnimation()
    {
        _animationLegIsPlaying = true;
        // Анимация левой ноги (вверх-вниз)
        _leftLegTween = legLTransform.DOLocalMoveY(movementRange, animationSpeed)
            .SetLoops(-1, LoopType.Yoyo) // Бесконечный цикл с возвратом
            .SetEase(Ease.InOutSine);

        // Анимация правой ноги
        _rightLegTween = legRTransform.DOLocalMoveY(-movementRange, animationSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    void StopLegAnimation()
    {
        _animationLegIsPlaying = false;
        // Останавливаем анимацию ног
        _leftLegTween.Kill();
        _rightLegTween.Kill();

        // Возвращаем ноги в исходное положение
        legLTransform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutSine);
        legRTransform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutSine);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
