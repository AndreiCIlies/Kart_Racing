using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
    public AudioSource engineSound; 
    public Rigidbody kartRigidbody; 

    public float minPitch = 0.8f; 
    public float maxPitch = 2.0f;
    public float maxSpeed = 50f; 
    public float decelerationRate = 0.95f;

    public KeyCode forwardKey1 = KeyCode.W;
    public KeyCode forwardKey2 = KeyCode.UpArrow; 
    public KeyCode backwardKey1 = KeyCode.S;
    public KeyCode backwardKey2 = KeyCode.DownArrow; 

    private bool isAccelerating; 

    void Start()
    {
        if (engineSound == null)
        {
            engineSound = GetComponent<AudioSource>();
        }

        if (kartRigidbody == null)
        {
            kartRigidbody = GetComponent<Rigidbody>();
        }

        engineSound.loop = true;
        engineSound.Play();
    }

    void Update()
    {
        isAccelerating = Input.GetKey(forwardKey1) || Input.GetKey(forwardKey2) ||
                         Input.GetKey(backwardKey1) || Input.GetKey(backwardKey2);

        float speed = kartRigidbody.linearVelocity.magnitude;

        if (isAccelerating)
        {
            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
        }
        else
        {
            engineSound.pitch = Mathf.Max(minPitch, engineSound.pitch * decelerationRate);
        }
    }
}
