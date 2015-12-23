using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour
{
    public float MinSpeed;
    public float MaxSpeed;
    public float LerpTime;
    public float PitchInfluence;
    public float PitchThreshold;

    private float speed;
    private float targetSpeed;
    private float oldPitch;
    private float currentVelocity;
    private AudioProcessor audioProcess;

	// Use this for initialization
	void Awake()
    {
        audioProcess = AudioProcessor.Instance;
        speed = MinSpeed;
        targetSpeed = speed;
        oldPitch = audioProcess.PitchValue;
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        // Change speed over pitch
        float pitch = audioProcess.PitchValue;
        float delta = pitch - oldPitch;
 
        if (Mathf.Abs(delta) > PitchThreshold)
        {
            float s = speed + delta * PitchInfluence;
            targetSpeed = Mathf.Clamp(s, MinSpeed, MaxSpeed);
        }

        speed = Mathf.SmoothDamp(speed, targetSpeed, ref currentVelocity, Time.deltaTime * LerpTime);

        // Compute camera new position and smooth translate
        Vector3 new_pos = transform.position + new Vector3(speed, 0f, 0f);
        transform.position = new_pos;
        // Vector3.Lerp(transform.position, new_pos, Time.deltaTime * LerpTime);

        oldPitch = pitch;
    }
}
