using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    // max angle (x & z) that the level can rotate to
    private float maxAngle = 75;
    // level turn speed
    private float turnSpeed = 12f;
    // level slerp speed; speed at which actual angle approaches target angle
    private float slerpSpeed = 4.0f;

    private MyoOrientation jointOrientation;
    private ThirdPersonCamera thirdPersonCamera;

    // most recently used key controls (set in Update and used in FixedUpdate)
    private Vector3 lastKeyOffsets;
    // most recent myo orientation
    private Vector3 lastMyoOrientation = new Vector3();
    private bool myoReady = false;

    private void Start()
    {
        jointOrientation = GetComponent<MyoOrientation>();
        thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
    }

    private void Update()
    {
        // update keyboard controller input
        lastKeyOffsets = GetKeyOffsets();

        // update myo input
        ThalmicHub hub = ThalmicHub.instance;
        ThalmicMyo thalmicMyo = jointOrientation.myo.GetComponent<ThalmicMyo>();
        if (hub.hubInitialized && thalmicMyo.isPaired && thalmicMyo.armSynced)
        {
            // myo ready, use myo controls
            lastMyoOrientation = jointOrientation.GetMyoRotation();
            myoReady = true;
        }
        else
        {
            myoReady = false;
        }
    }

    void FixedUpdate()
    {
        // target x, y, and z rotation of the level
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation;

        if (myoReady)
        {
            // myo ready, use myo controls
            targetRotation = RotateVectorAroundCamera(lastMyoOrientation);
        }
        else
        {
            // myo not ready, fall back to keyboard controls
            targetRotation = currentRotation + RotateVectorAroundCamera(lastKeyOffsets);
        }
        
        // angle that the level will rotate towards
        Quaternion targetQuat = Quaternion.Euler(targetRotation);
        // rotate (slerp) towards target x/z angle
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetQuat, Time.deltaTime * slerpSpeed);
        // limit rotation to maxAngle
        transform.rotation = Quaternion.Euler(LimitRotation(transform.rotation.eulerAngles));
    }

    // get keyboard control angle offsets
    private Vector3 GetKeyOffsets()
    {
        float xOff = 0;
        float yOff = 0;
        float zOff = 0;

        //Keys to rotate floor - similar to wasd but using ijkl instead
        if (Input.GetKey(KeyCode.I))
        {
            xOff += turnSpeed;
        }
        if (Input.GetKey(KeyCode.K))
        {
            xOff -= turnSpeed;
        }
        if (Input.GetKey(KeyCode.J))
        {
            zOff += turnSpeed;
        }
        if (Input.GetKey(KeyCode.L))
        {
            zOff -= turnSpeed;
        }

        //O and U always same regardless of camera angle
        if (Input.GetKey(KeyCode.U))
        {
            yOff += turnSpeed;
        }
        if (Input.GetKey(KeyCode.O))
        {
            yOff -= turnSpeed;
        }

        return new Vector3(xOff, yOff, zOff);
    }

    // limit rotation on the x and z axis
    private Vector3 LimitRotation(Vector3 vector)
    {
        // normalize angles
        float nx = normalizeAngle(vector.x);
        float ny = normalizeAngle(vector.y);
        float nz = normalizeAngle(vector.z);

        // x and z rotation hypotenuse (basically the total rotation between the two)
        float mag = Mathf.Sqrt(Mathf.Pow(nx, 2) + Mathf.Pow(nz, 2));
        if (mag > maxAngle)
        {
            // target rotation greater than limit; limit rotation
            // angle between x and z
            float angle = Mathf.Atan2(nz, nx);
            // compute limited x and z angles
            return new Vector3(Mathf.Cos(angle) * maxAngle, vector.y, Mathf.Sin(angle) * maxAngle);
        }
        else
        {
            // no limitation needed
            return vector;
        }
    }

    private Vector3 RotateVectorAroundCamera(Vector3 angle)
    {
        // rotate the rotation control by the camera angle
        // (in other words, make sure left is always left and right is always right from the player's perspective)
        return Quaternion.Euler(thirdPersonCamera.currentX, thirdPersonCamera.currentY, 0) * angle;
    }

    // Adjust the provided angle to be within a -180 to 180.
    float normalizeAngle(float angle)
    {
        if (angle > 180.0f)
        {
            return angle - 360.0f;
        }
        if (angle < -180.0f)
        {
            return angle + 360.0f;
        }
        return angle;
    }
}
