using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_control : MonoBehaviour
{
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

        /* Point the ship towards the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Set mouse Z to ship's Z
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset.z);
        */
    }
}
