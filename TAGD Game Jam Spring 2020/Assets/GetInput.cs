using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GetInput : MonoBehaviour
{
    private Vector2 _moveDirection = Vector2.zero;
    private float speed = 4f;
    private bool running = false;
    private Rigidbody2D _rb2d;

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = (running) ? 6f : 4f;
        transform.position += new Vector3(_moveDirection.x * Time.deltaTime * speed, _moveDirection.y * Time.deltaTime * speed, 0);
    }

    void OnMovement(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    void OnRun(InputValue value)
    {
        running = (value.isPressed) ? true : false;
    }

    void OnJump()
    {
        _rb2d.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }
}
