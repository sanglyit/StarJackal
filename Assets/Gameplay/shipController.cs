using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_control : MonoBehaviour
{
    /* 
    //Weapons
    Gun[] guns;
    bool shoot;

    // Ship movement speed
    float moveSpeed = 1f;
    // Borders of the playable area | bien gioi game
    [SerializeField] float worldMinX = -23f;
    [SerializeField] float worldMaxX = 23f;
    [SerializeField] float worldMinY = -23f;
    [SerializeField] float worldMaxY = 23f;

    //movement
    Vector2 moveDirection = Vector2.zero;

    [SerializeField] Vector3 rotationOffset = Vector3.zero;
    void Start()
    {
        guns = transform.GetComponentsInChildren<Gun>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Get input for movement direction
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // Normalize movement direction for diagonal consistency 
        moveDirection.Normalize();

        // Calculate movement vector using moveDirection and speed
        Vector2 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Constrain movement within world bounds 
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + movement.x, worldMinX, worldMaxX),
            Mathf.Clamp(transform.position.y + movement.y, worldMinY, worldMaxY),
            transform.position.z // Giu vi tri Z 
        );

        shoot = Input.GetKeyDown(KeyCode.Mouse0);
        if (shoot)
        {
            shoot = false;
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }
        
        */
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Gun gun;

    Vector2 moveDirection;
    Vector2 mousePosition;


    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            gun.Shoot();
        }

        moveDirection = new Vector2 (moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
