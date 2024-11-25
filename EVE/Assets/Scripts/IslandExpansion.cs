using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandExpansion : MonoBehaviour, IInteractable
{
    public int moneyRequired = 100;
    public bool built = false; //Maybe not needed
    public bool hasMoney = true; //Will need to be changed to false when able to check
    public GameObject bridge;
    public GameObject camera;
    private Transform cameraTransform;
    public SpriteRenderer rbSprite;

    private void Start()
    {
        cameraTransform = camera.transform;
    }

    private void Update()
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

    public void receiveInteract()
    {
     //Check if player or inventory has money
     if (hasMoney)
        {
            if (bridge)
            {
                bridge.SetActive(true);
                Debug.Log("You expanded your island!");
                gameObject.SetActive(false);
            }
        }
     
    }
}
