using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    private InputAction sprint;

    // Movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float sprintMultiplier = 2f; // Sprint speed multiplier
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

    private void Start()
    {
        EnablePlayerMovement();
    }

    private void OnEnable()
    {
        // Register input actions
        playerActionsAsset.Player.Jump.started += DoJump;
        playerActionsAsset.Player.Attack.started += DoAttack;
        move = playerActionsAsset.Player.Move;
        sprint = playerActionsAsset.Player.Sprint; // Add sprint input action
        playerActionsAsset.Player.Enable();
        PlayerHealth.OnPlayerDeath += DisablePlayerMovement;
        UIManager.OnGameOverMenuActivated += DisablePlayerAttack; // Subscribe to event
    }

    private void OnDisable()
    {
        // Unregister input actions
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Attack.started -= DoAttack;
        playerActionsAsset.Player.Disable();
        PlayerHealth.OnPlayerDeath -= DisablePlayerMovement;
        UIManager.OnGameOverMenuActivated -= DisablePlayerAttack; // Unsubscribe from event
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        float currentMovementForce = movementForce;

        // Check if sprint is pressed
        if (sprint.ReadValue<float>() > 0)
        {
            currentMovementForce *= sprintMultiplier;
        }

        forceDirection += moveInput.x * GetCameraRight(playerCamera) * currentMovementForce;
        forceDirection += moveInput.y * GetCameraForward(playerCamera) * currentMovementForce;

        if (jumpRequest)
        {
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

        // Update animator speed parameter
        animator.SetFloat("speed", rb.velocity.magnitude);

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
            jumpRequest = true;
        }
    }

    private bool IsGrounded()
    {
        Vector3 rayOrigin = this.transform.position + Vector3.up * 0.1f;
        float rayLength = 5f; // Adjust the length to ensure it reaches the ground

        bool grounded = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, groundLayer);

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, grounded ? Color.green : Color.red);

        return grounded;
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance == null || !UIManager.Instance.gameOverMenu.activeSelf) // Check if the game over menu is not active
        {
            animator.SetTrigger("attack");
        }
    }

    private void DisablePlayerAttack()
    {
        playerActionsAsset.Player.Attack.Disable(); // Disable attack input
    }

    private void EnablePlayerAttack()
    {
        playerActionsAsset.Player.Attack.Enable(); // Enable attack input
    }

    private void DisablePlayerMovement()
    {
        animator.enabled = false; // Disable the animator
        rb.constraints = RigidbodyConstraints.FreezeAll; // Freeze both position and rotation
    }

    private void EnablePlayerMovement()
    {
        animator.enabled = true; // Enable the animator
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Only freeze rotation, allow position to move
    }
}
