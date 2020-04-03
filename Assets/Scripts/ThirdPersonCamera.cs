using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Vector3 subject;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 18.0f;
    //Starting value set so camera spawns at most convenient
    //location for player to view the level
    private float value = 180.0f;
    private float rotateSpeed = 250;
    public System.Random rnd = new System.Random();
    private MyoPose myoPose;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        subject = new Vector3(25, value, 0);
        myoPose = FindObjectOfType<MyoPose>();
    }

    private void Update()
    {
        int concreteAudio = rnd.Next(1,4);

        //if R is pressed rotate camera 90 degrees clockwise
        if (Input.GetKeyDown(KeyCode.R) || myoPose.ConsumeWaveInIfDetected())
        {
            value += 90;
            FindObjectOfType<AudioManager>().Play("Rotate"+concreteAudio.ToString());
        }
        //T anti-clockwise
        if (Input.GetKeyDown(KeyCode.T) || myoPose.ConsumeWaveOutIfDetected())
        {
            value -= 90;
            FindObjectOfType<AudioManager>().Play("Rotate"+concreteAudio.ToString());
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
}
