using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public Transform player;
    public Transform portal;
    public Image pointerImage;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        pointerImage.enabled = false;
    }

    void Update()
    {
        if (portal.gameObject.activeSelf) // Check if the portal is active
        {
            ShowPointer();
        }
        else
        {
            pointerImage.enabled = false;
        }
    }

    void ShowPointer()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(portal.position);
        bool isOffScreen = screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height;

        if (isOffScreen)
        {
            pointerImage.enabled = true;

            // Calculate direction from player to portal
            Vector3 direction = (portal.position - player.position).normalized;

            // Convert direction to screen space and position the pointer
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointerImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90);

            // Set the pointer at the edge of the screen in the direction of the portal
            screenPosition.x = Mathf.Clamp(screenPosition.x, 50, Screen.width - 50);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 50, Screen.height - 50);
            pointerImage.rectTransform.position = screenPosition;
        }
        else
        {
            pointerImage.enabled = false; // Hide pointer when portal is in view
        }
    }
}
