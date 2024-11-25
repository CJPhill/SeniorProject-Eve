using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheCamera : MonoBehaviour
{

    public GameObject camera;
    public Transform cameraTransform;
    public SpriteRenderer rbSprite;


    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTransform = camera.transform;
        rbSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceCamera();
    }


    private void FaceCamera()
    {
        // Get the direction from the sprite to the camera
        Vector3 directionToCamera = cameraTransform.position - rbSprite.transform.position;

        // Set the y-component to zero to keep the rotation on the horizontal plane
        directionToCamera.y = 0;

        // Rotate the sprite to face the camera
        if (directionToCamera != Vector3.zero)
        {
            rbSprite.transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
}
