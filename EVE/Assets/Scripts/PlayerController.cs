using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool playerCanInteract;
    private GameObject interactObject = null;
    public GameObject interactSprite;

    public GameObject camera;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer rbSprite;

    public bool inventoryActive = false;

    [SerializeField] private GameObject inventory = null;
    public Image inventoryImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerCanMove = true;
        playerCanInteract = false;
        cameraTransform = camera.transform;
    }

    private void Update()
    {
        if (inventory)
        {
            checkInventory();
        }

        if (!DialogController.talking)
        {
            movePlayer();
            FaceCamera();
        }

        interactCheck();

        if (interactObject)
        {
            interactBought();
        }

        typingCheck();
    }

    private void checkInventory()
    {
        if (UserInput.instance.InventoryOpenClose)
        {
            if (GameManager.inventoryCamera != null)
            {
                GameManager.inventoryCamera.enabled = !GameManager.inventoryCamera.enabled;
            }

            if (inventoryImage != null)
            {
                inventoryImage.gameObject.SetActive(!inventoryImage.gameObject.activeSelf);
            }
        }
    }

    private void FaceCamera()
    {
        Vector3 directionToCamera = cameraTransform.position - rbSprite.transform.position;
        directionToCamera.y = 0;

        if (directionToCamera != Vector3.zero)
        {
            rbSprite.transform.rotation = Quaternion.LookRotation(-directionToCamera);
            interactSprite.transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }

    private void movePlayer()
    {
        if (playerCanMove)
        {
            moveInput.x = UserInput.instance.MoveInput.x;
            moveInput.y = UserInput.instance.MoveInput.y;
            moveInput.Normalize();

            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 movementDirection = (cameraRight * moveInput.x + cameraForward * moveInput.y) * moveSpeed;
            rb.velocity = new Vector3(movementDirection.x, rb.velocity.y, movementDirection.z);

            animator.SetFloat("xVelocity", moveInput.magnitude * moveSpeed);

            if (moveInput.x != 0)
            {
                rbSprite.flipX = moveInput.x < 0;
            }
        }
    }

    private void typingCheck()
    {
        GameObject computer = GameObject.FindWithTag("Computer");
        playerCanMove = (computer == null || !computer.activeInHierarchy);
    }

    private void interactCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerCanInteract)
        {
            animator.SetBool("isInteracting", true);
            StartCoroutine(ResetInteractAnimation());

            IInteractable interactable = interactObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactSprite.SetActive(false);
                interactable.receiveInteract();
            }
        }
    }

    private void interactBought()
    {
        if (!interactObject.activeInHierarchy)
        {
            playerCanInteract = false;
            interactObject = null;
        }
    }

    private IEnumerator ResetInteractAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("isInteracting", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            playerCanInteract = true;
            interactObject = collision.gameObject;
            interactSprite.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            playerCanInteract = false;
            interactObject = null;
            interactSprite.SetActive(false);
        }
    }
}
