using UnityEngine;
using System.Collections;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

public class PlayerMovement : MonoBehaviour
{
private float speed  = 5f;
private float cooldown = 0f;
private float distToGround;
Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update()
    {
        //Jump with ball if its on the ground
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            Vector3 jumpVelocity = new Vector3(0f,speed,0f);
            
            //Ensures ball maintains speed
            rb.velocity = rb.velocity + jumpVelocity; 
        }

        //Dash with ball
        //Dash should have significant cooldown (6s)
        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && cooldown <= 0) {
            rb.velocity = rb.velocity * speed;

            cooldown = 6f;
        }

    }
}