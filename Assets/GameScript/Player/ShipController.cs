using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship_control : MonoBehaviour
{
    Rigidbody2D shipRigidbody;
    public PlayerScriptableObject playerData; 

    public Camera cam;
    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 smoothvelocityInput;

    public Vector2 movement;
    private Vector2 mousePos;

    void Start()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();

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
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref smoothvelocityInput, 0.1f);
        shipRigidbody.velocity = smoothedMovementInput * playerData.MoveSpeed;

        //Look at mouse position
        Vector2 lookDir = mousePos - shipRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        shipRigidbody.rotation = angle;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
}
