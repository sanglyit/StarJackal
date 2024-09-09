using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship_control : MonoBehaviour
{
    Rigidbody2D shipRigidbody;
    //public PlayerScriptableObject playerData;
    PlayerStat player;

    public Camera cam;
    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public Vector2 smoothedMovementInput;
    [HideInInspector] public Vector2 smoothvelocityInput;

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public Vector2 mousePos;

    public float rayDistance = 0.5f;  // Distance to cast rays for border detection
    public LayerMask borderLayerMask; // LayerMask to specify which layer is considered a border
    void Start()
    {
        player = GetComponent<PlayerStat>();
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
        shipRigidbody.velocity = smoothedMovementInput * player.currentMoveSpeed;

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
