using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour
{
    public float MinSpeed;
    public float MaxSpeed;
    public float LerpTime;
    public float PitchInfluence;
    public float PitchThreshold;
    public float SmoothTime;
    public float DeltaTime;

    private float speed;
    private float oldPitch;
    private float currentVelocity;
    private AudioProcessor audioProcess;

	// Use this for initialization
	void Awake()
    {
        audioProcess = AudioProcessor.Instance;
        speed = MinSpeed;
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
            speed = Mathf.SmoothDamp(speed, Mathf.Clamp(s, MinSpeed, MaxSpeed), ref currentVelocity, SmoothTime, MaxSpeed, Time.deltaTime * DeltaTime);
        }
            
        // Compute camera new position and smooth translate
        Vector3 new_pos = transform.position + new Vector3(speed, 0f, 0f);
        //float smooth = Mathf.SmoothDamp(speed);
        transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime * LerpTime);

        oldPitch = pitch;
    }
}
