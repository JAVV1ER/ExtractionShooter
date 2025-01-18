using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsJoystickInputActive())
        {
            GetJoyStickInput();
        }
        else
        {
            GetUnityInput();
        }
    }
    bool IsJoystickInputActive()
    {
        // Проверяем, есть ли отклонение джойстика
        return Mathf.Abs(VirtualJoystick.GetAxis("Horizontal", 1)) > 0.1f || Mathf.Abs(VirtualJoystick.GetAxis("Vertical", 1)) > 0.1f;
    }

    void GetJoyStickInput()
    {
        _movement.x = VirtualJoystick.GetAxis("Horizontal", 1);
        _movement.y = VirtualJoystick.GetAxis("Vertical", 1);
    }

    void GetUnityInput()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical"); 
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }
}
