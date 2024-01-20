using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PandaController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    public bool isJumping = false;
    public bool isGrounded = false;
    float leftRightDirection = 0;
    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;

    public void OnMove(InputAction.CallbackContext context) {
        leftRightDirection = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context) {
        //If can jump, call the jump function
        if (!isJumping) {
            isJumping = true;
            Vector2 currentVelocity = rb.velocity;
            currentVelocity.y = jumpSpeed;
            rb.velocity = currentVelocity;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        //If triggered with an object, interact with it.
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("direction: " + leftRightDirection);
        //leftRightDirection = inputs.
        rb.velocity = new Vector2(leftRightDirection * speed, rb.velocity.y);
    }

    // Check if an object is below the player.
    private void UpdateGrounded() {
        Vector2 raycastVector = transform.TransformPoint(transform.position);
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.Raycast(raycastVector, Vector2.down, (capsuleCollider.size.y/2) + 0.05f);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 0.1f, false);
        if (isGrounded && !wasGrounded && isJumping) {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2d(Collider other) {
        // Store the triggered object
    }

    private void OnTriggerExit2D(Collider2D other) {
        // Delete the triggered object
    }
}
