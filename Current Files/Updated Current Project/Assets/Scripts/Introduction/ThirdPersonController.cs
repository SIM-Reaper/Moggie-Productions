using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;
    private bool jumpRequest = false;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private LayerMask groundLayer; // Add a layer mask for the ground

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.Jump.started += DoJump;
        playerActionsAsset.Player.Attack.started += DoAttack;
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Attack.started -= DoAttack;
        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        forceDirection += moveInput.x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += moveInput.y * GetCameraForward(playerCamera) * movementForce;

        if (jumpRequest)
        {
            Debug.Log("Jump requested");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            if (direction.x < 0)
                spriteRenderer.flipX = true;
            else if (direction.x > 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            Debug.Log("Grounded and jumping");
            jumpRequest = true;
        }
        else
        {
            Debug.Log("Not grounded, can't jump");
        }
    }

    private bool IsGrounded()
    {
        // Set the ray origin to just slightly above the player's feet
        Vector3 rayOrigin = this.transform.position + Vector3.up * 0.1f;
        float rayLength = 0.3f; // Adjust the length to ensure it reaches the ground

        // Cast the ray downward to detect the ground
        bool grounded = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, groundLayer);
        Debug.Log("IsGrounded: " + grounded);

        // Visualize the ray in the Scene view for debugging purposes
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, grounded ? Color.green : Color.red);

        return grounded;
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("attack");
    }
}