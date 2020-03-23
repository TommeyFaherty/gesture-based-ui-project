using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PlayerMovement : MonoBehaviour
{
    private float jumpForce = 10f;
    private float dashForce = 15f;
    private float cooldown = 0f;
    private float distToGround;
    Rigidbody rb;

    private Pose _lastPose = Pose.Unknown;

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
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoJump = false;
        bool myoDash = false;

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.Fist)
            {
                myoJump = true;
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                myoDash = true;
            }

            ExtendUnlockAndNotifyUserAction(thalmicMyo);
        }

        //Jump with ball if its on the ground
        if ((Input.GetKeyDown(KeyCode.Space) || myoJump) && IsGrounded()) {
            Vector3 jumpVelocity = new Vector3(0f,jumpForce,0f);
            
            //Ensures ball maintains speed
            rb.velocity = rb.velocity + jumpVelocity; 
        }

        //Dash with ball
        //Dash should have significant cooldown (6s)
        if(cooldown > 0)
            cooldown -= Time.deltaTime;

        if((Input.GetKeyDown(KeyCode.LeftShift) || myoDash) && cooldown <= 0) {
            rb.velocity *= dashForce;

            cooldown = 6f;
        }
    }

    private void FixedUpdate()
    {
        // add some extra downward force to the player so that it doesn't bounce around the level as much
        rb.AddForce(1, -100, 1);
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }
}