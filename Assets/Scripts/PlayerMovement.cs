using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float jumpForce = 7f;
    private float dashForce = 5f;
    private float cooldown = 0f;
    private float distToGround;
    Rigidbody rb;

    private MyoPose myoPose;
    private LevelController levelController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        myoPose = FindObjectOfType<MyoPose>();
        levelController = FindObjectOfType<LevelController>();
    }

    // normal angle of the level
    private Vector3 GetLevelNormal()
    {
        return levelController.transform.up;
    }

    bool IsGrounded() {
        // raycast towards level
        Vector3 inverseNormal = GetLevelNormal() * -1;
        return Physics.Raycast(transform.position, inverseNormal, distToGround + 0.1f);
    }

    void Update()
    {
        //Jump with ball if its on the ground
        if ((Input.GetKeyDown(KeyCode.Space) || myoPose.ConsumeFistIfDetected()) && IsGrounded()) {
            Vector3 jumpVelocity = GetLevelNormal() * jumpForce;
            
            //Ensures ball maintains speed
            rb.velocity = rb.velocity + jumpVelocity; 
        }

        //Dash with ball
        //Dash should have significant cooldown (6s)
        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if((Input.GetKeyDown(KeyCode.LeftShift) || myoPose.ConsumeDoubleTapIfDetected()) && cooldown <= 0) {
            rb.velocity *= dashForce;

            cooldown = 6f;
        }

        //Restart level if ball is falling of the map
        if(rb.position.y <= -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        // add some extra downward force to the player so that it doesn't bounce around the level as much
        rb.AddForce(1, -50, 1);
    }
}