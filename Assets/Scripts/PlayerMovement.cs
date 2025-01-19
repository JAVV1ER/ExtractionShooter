using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [SerializeField] private SpriteRenderer headSpriteRenderer;

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
