using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    // max angle (x & z) that the level can rotate to
    private float maxAngle = 70;
    // level turn speed
    private float turnSpeed = 0.65f;
    // level slerp speed; speed at which actual angle approaches target angle
    private float slerpSpeed = 3.0f;

    private JointOrientation jointOrientation;
    private ThirdPersonCamera thirdPersonCamera;

    private void Start()
    {
        jointOrientation = GetComponent<JointOrientation>();
        thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
    }

    void FixedUpdate()
    {
        ThalmicHub hub = ThalmicHub.instance;
        ThalmicMyo thalmicMyo = jointOrientation.myo.GetComponent<ThalmicMyo>();

        // target x, y, and z rotation of the level
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation;

        if (hub.hubInitialized && thalmicMyo.isPaired && thalmicMyo.armSynced)
        {
            // myo ready, use myo controls
            targetRotation = RotateVectorAroundCamera(jointOrientation.GetMyoRotation());
        }
        else
        {
            // myo not ready, fall back to keyboard controls
            targetRotation = currentRotation + RotateVectorAroundCamera(GetKeyOffsets());
        }
        
        // angle that the level will rotate towards (limited to maxAngle)
        Quaternion targetRot = Quaternion.Euler(LimitRotation(targetRotation));
        // rotate (slerp) towards target x/z angle
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRot, Time.deltaTime * slerpSpeed);
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
        // x and z rotation hypotenuse (basically the total rotation between the two)
        float xzHypot = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.z, 2));
        if (xzHypot > maxAngle)
        {
            // target rotation greater than limit; limit rotation
            // angle between x and z
            float angle = Mathf.Atan2(vector.z, vector.x);
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
        return Quaternion.Euler(0, thirdPersonCamera.currentY, 0) * angle;
    }
}
