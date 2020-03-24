using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class ThirdPersonCamera : MonoBehaviour
{
    public Vector3 subject;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 18.0f;
    private float value = 0.0f;
    private float rotateSpeed = 90;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        subject = new Vector3();
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
        
        float rotateAmount = rotateSpeed * Time.deltaTime;
        // snap to target rotation if close enough (stops camera from jittering)
        if (Mathf.Abs(value - subject.y) <= rotateAmount)
        {
            subject.y = value;
        }
        //Camera glides when rotating instead of jumping to position
        if (subject.y < value)
        {
            subject.y += rotateAmount;
        }
        else if(subject.y > value)
        {
            subject.y -= rotateAmount;
        }
    }

    private void LateUpdate() 
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(subject.x, subject.y, 0);
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
