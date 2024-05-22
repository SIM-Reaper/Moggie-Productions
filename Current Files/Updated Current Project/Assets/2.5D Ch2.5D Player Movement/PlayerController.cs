using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody theRB;
    public float moveSpeed, jumpForce;

    private Vector2 moveInput;

    public LayerMask whatIsGround;
    public Transform groundPoint;
    private bool isGrounded;

    void Start()
    {
        if (theRB == null)
        {
            Debug.LogError("Rigidbody not assigned!");
        }
    }

    void FixedUpdate()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);

        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, whatIsGround))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("NOT Grounded");
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump button pressed and grounded. Jumping.");
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
        }
    }
}
