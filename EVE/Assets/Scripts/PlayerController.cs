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
    private Animator animator;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Interaction")]
    private bool playerCanInteract;
    private GameObject interactObject = null;


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
        animator = GetComponentInChildren<Animator>();
        playerCanMove = true;
        playerCanInteract = false;

    }

    private void Update()
    {
        checkMenu();
        checkInventory();
        if(!DialogController.talking){
            movePlayer();
            //FaceCamera();
        }
        interactCheck();
        typingCheck();
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

            rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);

            // Calculate movement magnitude
            float movementMagnitude = moveInput.magnitude;

            // Set Animator parameter for blend tree
            animator.SetFloat("xVelocity", movementMagnitude * moveSpeed);

            //Sprite Flipping
            if (moveInput.x != 0 && moveInput.x < 0)
            {
                rbSprite.flipX = true;
            }
            else if (moveInput.x != 0 && moveInput.x > 0)
            {
                rbSprite.flipX = false;
            }
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
                Debug.Log("Can not move");
            }
            else
            {
                playerCanMove = true;
                
            }
        }
        else
        {
            playerCanMove = true;
            Debug.Log("Can move");
        }
    }

    private void interactCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerCanInteract)
        {
            // Trigger the interact animation
            animator.SetBool("isInteracting", true);

            // Optionally, reset the animation after a short delay
            StartCoroutine(ResetInteractAnimation());

            // Check if the collided object has a component that implements IInteractable
            IInteractable interactable = interactObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.receiveInteract();
            }
        }
    }

    private IEnumerator ResetInteractAnimation()
    {
        yield return new WaitForSeconds(1.0f); // Adjust the duration to match your animation length
        animator.SetBool("isInteracting", false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            //Show Prompt to Interact
            playerCanInteract = true;
            interactObject = collision.gameObject;
            
            
        }
    }

    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            playerCanInteract = false;
            interactObject = null;
        }
    }


}
