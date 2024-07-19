using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship_control : MonoBehaviour
{
    [SerializeField] private float _speed;

    public Camera cam;
    private Rigidbody2D _shipRigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _smoothvelocityInput;

    public Vector2 movement;
    private Vector2 mousePos;

    void Start()
    {
        _shipRigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //di chuyen
        _shipRigidbody.velocity = _movementInput * _speed;

        //Look at mouse position
        Vector2 lookDir = mousePos - _shipRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        _shipRigidbody.rotation = angle;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
