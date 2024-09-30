using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (target != null)  // Check if the target is valid
        {
            transform.position = target.position + offset;
        }
    }
}
