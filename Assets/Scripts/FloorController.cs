using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    //Variables - specifying floor rotation per axis
    private float IKeysXAxis = 0.5f, IKeysZAxis = 0f;
    private float JKeysXAxis = 0f, JKeysZAxis = 0.5f;
    private float KKeysXAxis = -0.5f, KKeysZAxis = 0f;
    private float LKeysXAxis = 0f, LKeysZAxis = -0.5f;
    private float xHolder, zHolder;

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
        if(Input.GetKey(KeyCode.I))
        {
            transform.Rotate(IKeysXAxis,0,IKeysZAxis);
        }
        if(Input.GetKey(KeyCode.K))
        {
            transform.Rotate(KKeysXAxis,0,KKeysZAxis);
        }
        if(Input.GetKey(KeyCode.J))
        {
            transform.Rotate(JKeysXAxis,0,JKeysZAxis);
        }
        if(Input.GetKey(KeyCode.L))
        {
            transform.Rotate(LKeysXAxis,0,LKeysZAxis);
        }

        //O and U always same regardless of camera angle
        if(Input.GetKey(KeyCode.U))
        {
            transform.Rotate(0,0.5f,0);
        }
        if(Input.GetKey(KeyCode.O))
        {
            transform.Rotate(0,-0.5f,0);
        }
    }
}
