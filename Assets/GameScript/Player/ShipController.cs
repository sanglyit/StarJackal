using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship_control : MonoBehaviour
{
    Rigidbody2D shipRigidbody;
    PlayerStat player;

    public Camera cam;
    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public Vector2 smoothedMovementInput;
    [HideInInspector] public Vector2 smoothVelocityInput;

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public Vector2 mousePos;

    public float movementSmoothing = 0.1f; // Movement smoothing time
    public float rotationSmoothing = 0.01f; // Rotation smoothing time
    private float currentRotation;

    void Start()
    {
        player = GetComponent<PlayerStat>();
        shipRigidbody = GetComponent<Rigidbody2D>();
        shipRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;  // Enable interpolation for smoother physics
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // Smooth Movement
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, ref smoothVelocityInput, movementSmoothing);
        Vector2 targetPosition = shipRigidbody.position + smoothedMovementInput * player.CurrentMoveSpeed * Time.fixedDeltaTime;
        shipRigidbody.MovePosition(targetPosition);

        // Smooth Rotation
        Vector2 lookDir = mousePos - shipRigidbody.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        // Smoothly rotate towards the target angle
        currentRotation = Mathf.LerpAngle(shipRigidbody.rotation, targetAngle, rotationSmoothing);
        shipRigidbody.MoveRotation(currentRotation);

        // Reset any unwanted angular velocity caused by collisions
        shipRigidbody.angularVelocity = 0f;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
}
