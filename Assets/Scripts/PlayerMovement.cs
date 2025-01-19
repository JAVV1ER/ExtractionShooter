using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;
using UnityEngine.Serialization;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private Transform legLTransform;
    [SerializeField] private Transform legRTransform;

    private Tween _leftLegTween;
    private Tween _rightLegTween;
    [SerializeField] private float animationSpeed = 0.5f; // Скорость движения ног
    [SerializeField] private float movementRange = 0.2f;
    private bool _isMoving;
    
    private Rigidbody2D _rb;
    private Vector2 _movement;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if(!_rb) Debug.LogError("No rigidbody attached");
    }

    void Update()
    {
        //Переключаем, если в эдиторе нужно бегать на клавиатуре или джойстике
#if !UNITY_EDITOR
        GetJoyStickInput();
#else
        GetUnityInput();
#endif
        
        LegAnimation();
    }

    void GetJoyStickInput()
    {
        _movement.x = VirtualJoystick.GetAxis("Horizontal", 1);
        _movement.y = VirtualJoystick.GetAxis("Vertical", 1);
        FlipSprites();
    }

    void GetUnityInput()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
        
        FlipSprites();
    }

    bool IsMoving()
    {
        return _movement != Vector2.zero;
    }

    void LegAnimation()
    {
        // Проверка, двигается ли персонаж
        bool moving = IsMoving();

        if (moving && !_isMoving)
        {
            StartLegAnimation();
        }
        else if (!moving && _isMoving)
        {
            StopLegAnimation();
        }

        _isMoving = moving;
    }
    void StartLegAnimation()
    {
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
        // Останавливаем анимацию ног
        _leftLegTween.Kill();
        _rightLegTween.Kill();

        // Возвращаем ноги в исходное положение
        legLTransform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutSine);
        legRTransform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutSine);
    }

    void FlipSprites()
    {
        if (_movement.x > 0)
        {
            bodySpriteRenderer.flipX = false;
            headSpriteRenderer.flipX = false;
        }
        else if (_movement.x < 0) // Только при движении влево
        {
            bodySpriteRenderer.flipX = true;
            headSpriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }
}
