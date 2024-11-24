using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform target; // Target to orbit around
    public float orbitDistance = 10f; // Distance from the target
    public float orbitSpeed = 5f; // Speed for smooth transitions

    private Vector3 currentOffset; // The current offset of the camera
    private Vector3 targetPosition; // Target position for smooth transition

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned for the camera to orbit!");
            return;
        }

        // Calculate initial offset
        currentOffset = transform.position - target.position;
    }

    void Update()
    {
        // Check for input to rotate around the target
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RotateAroundTarget(90); // Rotate 90 degrees counter-clockwise
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            RotateAroundTarget(-90); // Rotate 90 degrees clockwise
        }

        // Smoothly move the camera to the new position
        Vector3 desiredPosition = target.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * orbitSpeed);

        // Ensure the camera is always looking at the target
        transform.LookAt(target);
    }

    void RotateAroundTarget(float angle)
    {
        // Rotate the offset vector around the target's Y-axis
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        currentOffset = rotation * currentOffset; // Rotate the offset vector
    }
}
