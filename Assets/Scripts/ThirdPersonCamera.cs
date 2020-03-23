using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float currentX = 25f;
    public float currentY = 0.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 18.0f;
    private float value = 0.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        //if R is pressed rotate camera 90 degrees clockwise
        //T anti-clockwise
        if(Input.GetKeyDown(KeyCode.R))
        {
            value += 90;
            Debug.Log("R was pressed");
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            value -= 90;
            Debug.Log("T was pressed");
        }

        //Camera glides when rotating instead of jumping to position
        if(currentY < value)
        {
            currentY += 5;
        }
        else if(currentY > value)
        {
            currentY -= 5;
        }

        //currentY = Mathf.Clamp(currentY,Y_ANGLE_MIN,Y_ANGLE_MAX);
    }

    private void LateUpdate() 
    {

        Vector3 dir = new Vector3(0,0,-distance);
        Quaternion rotation = Quaternion.Euler(currentX,currentY,0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
