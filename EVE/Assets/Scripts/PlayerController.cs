using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float groundDist;
    public int playerHeight;

    [Header("Player Movement")]
    private Vector2 moveInput;
    private Transform cameraTransform;
    private bool playerCanMove;


    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer rbSprite;

    public bool menuActive = false;
    public bool inventoryActive = false;

    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject inventory = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCanMove = true;
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        checkMenu();
        checkInventory();
        if(!DialogController.talking){
            movePlayer();
            FaceCamera();
        }
        // typingCheck();
    }

    /// <summary>
    /// ITEMS RELATED to UI/Keys
    /// </summary>
    /// 

    private void checkMenu()
    {
        if (UserInput.instance.MenuOpenClose)
        {
            menuActive = !menuActive;
            Debug.Log("Menu Active: " + menuActive);
            menu.SetActive(menuActive);
        }
    }

    private void checkInventory(){
        if (UserInput.instance.InventoryOpenClose)
        {
            inventoryActive = !inventoryActive;
            inventory.SetActive(inventoryActive);
        }
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
            rbSprite.transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }


    /// <summary>
    /// Movement related code
    /// </summary>
    private void movePlayer()

    {
        if (playerCanMove)
        {
            // Get movement input
            moveInput.x = UserInput.instance.MoveInput.x;
            moveInput.y = UserInput.instance.MoveInput.y;
            moveInput.Normalize();

            // Calculate movement relative to the camera's facing direction
            Vector3 moveDirection = cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x;
            moveDirection.y = 0; // Keep movement horizontal

            // Apply velocity based on moveDirection
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

            // Sprite flipping based on input direction
            if (moveInput.x < 0) rbSprite.flipX = true;
            else if (moveInput.x > 0) rbSprite.flipX = false;
        }
    }


    /// <summary>
    /// Scripts dealing with collision/Interaction
    /// </summary>
    /// 

    private void typingCheck()
    {
        if (GameObject.FindWithTag("Computer") != null)
        {
            GameObject computer = GameObject.FindWithTag("Computer");
            Debug.Log("Object with tag exists in the scene.");
            if (computer.activeInHierarchy)
            {
                playerCanMove = false;
            }
            else
            {
                playerCanMove = true;
            }
        }
        else
        {
            Debug.Log("Object with tag does not exist in the scene.");
            playerCanMove = true;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            //Show Prompt to Interact
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Interactable"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if the collided object has a component that implements IInteractable
                IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.receiveInteract();
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            //Hid Prompt to Interact
        }
    }


}
