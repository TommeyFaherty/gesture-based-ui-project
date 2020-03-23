using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

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

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = FindObjectOfType<ThalmicMyo>();

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.WaveIn)
            {
                value += 90;
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                value -= 90;
            }

            ExtendUnlockAndNotifyUserAction(thalmicMyo);
        }

        //if R is pressed rotate camera 90 degrees clockwise
        //T anti-clockwise
        if (Input.GetKeyDown(KeyCode.R))
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
