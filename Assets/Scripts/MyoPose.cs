using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class MyoPose : MonoBehaviour
{
    // remember which poses have been used
    private bool fingersSpread;
    private bool fist;
    private bool doubleTap;
    private bool waveIn;
    private bool waveOut;

    // last recognized pose; prevents the same pose from being consumed twice
    private Pose _lastPose = Pose.Unknown;

    // Update is called once per frame
    void Update()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = FindObjectOfType<ThalmicMyo>();

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            // update pose variables
            switch (thalmicMyo.pose)
            {
                case Pose.FingersSpread:
                    fingersSpread = true;
                    break;
                case Pose.Fist:
                    fist = true;
                    break;
                case Pose.DoubleTap:
                    doubleTap = true;
                    break;
                case Pose.WaveIn:
                    waveIn = true;
                    break;
                case Pose.WaveOut:
                    waveOut = true;
                    break;
            }

            ExtendUnlockAndNotifyUserAction(thalmicMyo);
        }
    }

    // consume the fingers spread pose if detected, returning true.
    // returns false if fingers spread was not detected recently
    public bool ConsumeFingersSpreadIfDetected()
    {
        if (fingersSpread)
        {
            fingersSpread = false;
            return true;
        }

        return false;
    }

    // consume the fist pose if detected, returning true.
    // returns false if fingers spread was not detected recently
    public bool ConsumeFistIfDetected()
    {
        if (fist)
        {
            fist = false;
            return true;
        }

        return false;
    }

    // consume the double tap pose if detected, returning true.
    // returns false if fingers spread was not detected recently
    public bool ConsumeDoubleTapIfDetected()
    {
        if (doubleTap)
        {
            doubleTap = false;
            return true;
        }

        return false;
    }

    // consume the wave in pose if detected, returning true.
    // returns false if fingers spread was not detected recently
    public bool ConsumeWaveInIfDetected()
    {
        if (waveIn)
        {
            waveIn = false;
            return true;
        }

        return false;
    }

    // consume the wave out pose if detected, returning true.
    // returns false if fingers spread was not detected recently
    public bool ConsumeWaveOutIfDetected()
    {
        if (waveOut)
        {
            waveOut = false;
            return true;
        }

        return false;
    }

    // extend the unlock if ThalmcHub's locking policy is standard,
    // and notifies the given myo that a user action was recognized.
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
