using UnityEngine;
using static AngleUtils;

public class LevelController : MonoBehaviour
{
    // max angle (x & z) that the level can rotate to
    private readonly float maxAngle = 80;
    // level turn speed
    private readonly float turnSpeed = 1.0f;
    // level slerp speed; speed at which actual angle approaches target angle
    private readonly float slerpSpeed = 0.1f;

    // used to check the orientation of the myo armband
    private MyoOrientation myoOrientation;
    // reference to the camera for relative level movement
    private ThirdPersonCamera thirdPersonCamera;

    // most recently used key controls (set in Update and used in FixedUpdate)
    private Vector3 lastKeyOffsets;
    private Vector3 keyTargetRot;
    // most recent myo orientation
    private Vector3 lastMyoOrientation = new Vector3();
    private bool myoReady = false;

    private void Start()
    {
        myoOrientation = FindObjectOfType<MyoOrientation>();
        thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
        keyTargetRot = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        // update keyboard controller input
        lastKeyOffsets = GetKeyOffsets();

        // update myo input
        ThalmicHub hub = ThalmicHub.instance;
        ThalmicMyo thalmicMyo = FindObjectOfType<ThalmicMyo>();
        if (hub.hubInitialized && thalmicMyo.isPaired && thalmicMyo.armSynced)
        {
            // myo ready, use myo controls
            lastMyoOrientation = myoOrientation.GetMyoRotation();
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
            targetRotation = RotateVectorAroundVector(lastMyoOrientation, thirdPersonCamera.subject);
        }
        else
        {
            // myo not ready, fall back to keyboard controls
            keyTargetRot += RotateVectorAroundVector(GetKeyOffsets(), thirdPersonCamera.subject);
            keyTargetRot = LimitRotation(keyTargetRot, maxAngle);

            targetRotation = keyTargetRot;
        }
        
        // angle that the level will rotate towards
        Quaternion targetQuat = Quaternion.Euler(targetRotation);
        // rotate (slerp) towards target x/z angle
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetQuat, slerpSpeed);
        // limit rotation to maxAngle
        transform.rotation = Quaternion.Euler(LimitRotation(transform.rotation.eulerAngles, maxAngle));
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
            xOff += 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            xOff -= 1;
        }
        if (Input.GetKey(KeyCode.J))
        {
            zOff += 1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            zOff -= 1;
        }

        //O and U always same regardless of camera angle
        if (Input.GetKey(KeyCode.U))
        {
            yOff += 1;
        }
        if (Input.GetKey(KeyCode.O))
        {
            yOff -= 1;
        }

        return new Vector3(xOff, yOff, zOff).normalized * turnSpeed;
    }
}
