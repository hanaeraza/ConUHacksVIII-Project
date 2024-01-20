using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Footsteps))]
// Controls all player movement and movement-related animations.
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject camHolder;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sensitivity;
    [SerializeField] float maxForce;
    [SerializeField] float jumpSpeed = 9;
    [SerializeField] float dodgeForce = 2000;
    [SerializeField] float dodgeDuration = 0.12f;
    [SerializeField] float dodgeCooldown = 2;
    [SerializeField] float maxHealth = 5;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Canvas uiCanvas;
    private Rigidbody playerRigidbody;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    //private DisplayDamage displayDamage;
    private Footsteps footsteps;
    //private EndGame endGame;
    //private ItemHandler itemHandler;
    private Vector2 move, look;
    private float lookRotation;
    public bool isGrounded;
    private bool isSprinting;
    private bool isCrouching;
    private bool isCeiling;
    private bool isJumping;
    private float currentHealth;
    private bool canDodge = true;
    public bool IsGrounded { get { return isGrounded; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsCrouching { get { return isCrouching; } }
    public Vector2 MoveVector { get { return move; } }
    public float CurrentSpeed;

    // Anyone can enable or disable player movement.
    public bool CanMove = true;

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context) {
        isSprinting = context.ReadValueAsButton();
        footsteps.IsSprinting = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context) {
        isCrouching = context.ReadValueAsButton();
        footsteps.IsCrouching = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (CanMove) {
            Jump();
        }
    }

    public void OnLook(InputAction.CallbackContext context) {
        look = context.ReadValue<Vector2>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        playerRigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        //displayDamage = GetComponent<DisplayDamage>();
        footsteps = GetComponent<Footsteps>();
        //endGame = FindObjectOfType<EndGame>();
        //itemHandler = GetComponentInChildren<ItemHandler>();
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Move();
        UpdateGrounded();
        UpdateCeiling();
    }

    private void Update() {
        if (isCrouching) {
            animator.SetBool("isCrouching", isCrouching);
        } 
        else if (!isCeiling) {
            animator.SetBool("isCrouching", isCrouching);
        }

        if (CanMove && move != Vector2.zero) {
            footsteps.IsMoving = true;
        }
        else {
            footsteps.IsMoving = false;
        }

        //healthText.text = "HP: " + currentHealth + "/" + maxHealth;
    }

    void LateUpdate()
    {
        Look();
    }

    // Check what kind of walk the player is in and move accordingly.
    private void Move()
    {
        if (CanMove) {
            // Find target velocity.
            Vector3 currentVelocity = playerRigidbody.velocity;
            Vector3 targetVelocity = new Vector3(move.x, 0, move.y);

            if (isCrouching) {
                targetVelocity *= crouchSpeed;
                CurrentSpeed = crouchSpeed;
            } else if (isSprinting) {
                targetVelocity *= sprintSpeed;
                CurrentSpeed = sprintSpeed;
            } else {
                targetVelocity *= walkSpeed;
                CurrentSpeed = walkSpeed;
            }

            targetVelocity = transform.TransformDirection(targetVelocity);
            // Find velocity change without taking Y into account.
            Vector3 velocityChange = targetVelocity - currentVelocity;
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
            //Vector3.ClampMagnitude(velocityChange, maxForce);

            playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    // Check if an object is below the player.
    private void UpdateGrounded() {
        Vector3 raycastVector = transform.TransformPoint(capsuleCollider.center);
        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(raycastVector, Vector3.down, (capsuleCollider.height/2) + 0.05f);
        if (isGrounded && !wasGrounded && isJumping) {
            isJumping = false;
            footsteps.PlayLand();
        }
    }

    // Check if an object is above the player - used for exiting crouch.
    private void UpdateCeiling() {
        Vector3 raycastVector = transform.TransformPoint(capsuleCollider.center);
        isCeiling = Physics.Raycast(raycastVector, Vector3.up, (capsuleCollider.height/2) + 1f);
    }

    private void Jump() {
        Vector3 currentVelocity = playerRigidbody.velocity;
        if (isGrounded) {
            currentVelocity.y = jumpSpeed;
            playerRigidbody.velocity = currentVelocity;
            isJumping = true;
            footsteps.PlayJump();
        }
    }

    private void Look()
    {
        // Rotate the player.
        transform.Rotate(Vector3.up * look.x * sensitivity);

        // Rotate the camera.
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }
}
