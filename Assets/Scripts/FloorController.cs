using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    // target x, y, and z rotation of the level
    private float xRot, yRot, zRot;
    // max angle (x & z) that the level can rotate to
    private float maxAngle = 65;
    // level turn speed
    private float turnSpeed = 0.65f;
    // level lerp speed; speed at which actual angle approaches target angle
    private float lerpSpeed = 0.08f;

    //Variables - specifying floor rotation per axis
    private float IKeysXAxis, IKeysZAxis;
    private float JKeysXAxis, JKeysZAxis;
    private float KKeysXAxis, KKeysZAxis;
    private float LKeysXAxis, LKeysZAxis;
    private float xHolder, zHolder;

    private void Start()
    {
        xRot = yRot = zRot = 0;

        // initialise floor rotations per axis
        IKeysXAxis = turnSpeed;
        IKeysZAxis = 0f;

        JKeysXAxis = 0f;
        JKeysZAxis = turnSpeed;

        KKeysXAxis = -turnSpeed;
        KKeysZAxis = 0f;

        LKeysXAxis = 0f;
        LKeysZAxis = -turnSpeed;
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Upon rotating anti-clockwise controls are changed to match users perspective
            xHolder = IKeysXAxis;
            zHolder = IKeysZAxis;

            IKeysXAxis = JKeysXAxis;
            IKeysZAxis = JKeysZAxis;

            JKeysXAxis = KKeysXAxis;
            JKeysZAxis = KKeysZAxis;

            KKeysXAxis = LKeysXAxis;
            KKeysZAxis = LKeysZAxis;

            LKeysXAxis = xHolder;
            LKeysZAxis = zHolder;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Upon rotating clockwise controls are changed to match users perspective
            xHolder = IKeysXAxis;
            zHolder = IKeysZAxis;

            IKeysXAxis = LKeysXAxis;
            IKeysZAxis = LKeysZAxis;

            LKeysXAxis = KKeysXAxis;
            LKeysZAxis = KKeysZAxis;

            KKeysXAxis = JKeysXAxis;
            KKeysZAxis = JKeysZAxis;

            JKeysXAxis = xHolder;
            JKeysZAxis = zHolder;
        }
    }

    void FixedUpdate()
    {
        //Keys to rotate floor - similar to wasd but using ijkl instead
        if (Input.GetKey(KeyCode.I))
        {
            xRot += IKeysXAxis;
            zRot += IKeysZAxis;
        }
        if(Input.GetKey(KeyCode.K))
        {
            xRot += KKeysXAxis;
            zRot += KKeysZAxis;
        }
        if(Input.GetKey(KeyCode.J))
        {
            xRot += JKeysXAxis;
            zRot += JKeysZAxis;
        }
        if(Input.GetKey(KeyCode.L))
        {
            xRot += LKeysXAxis;
            zRot += LKeysZAxis;
        }

        //O and U always same regardless of camera angle
        if(Input.GetKey(KeyCode.U))
        {
            yRot += turnSpeed;
        }
        if(Input.GetKey(KeyCode.O))
        {
            yRot -= turnSpeed;
        }

        // limit rotation on the x and z axis
        LimitRotation();

        // rotate (lerp) towards target x/z angle
        Quaternion targetRot = Quaternion.Euler(xRot, yRot, zRot);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.time * lerpSpeed);
    }

    // limit rotation on the x and z axis
    private void LimitRotation()
    {
        // x and z rotation hypotenuse (basically the total rotation between the two)
        float xzHypot = Mathf.Sqrt(Mathf.Pow(xRot, 2) + Mathf.Pow(zRot, 2));
        if (xzHypot > maxAngle)
        {
            // target rotation greater than limit; limit rotation
            // angle between x and z
            float angle = Mathf.Atan2(zRot, xRot);
            // compute limited x and z angles
            xRot = Mathf.Cos(angle) * maxAngle;
            zRot = Mathf.Sin(angle) * maxAngle;
        }
    }
}
