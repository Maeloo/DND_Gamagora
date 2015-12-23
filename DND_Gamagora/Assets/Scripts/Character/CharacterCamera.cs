using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour
{
    public float MinSpeed = 3f;
    public float MaxSpeed = 10f;
    public float LerpTime = 0.01f;
    public float PitchInfluence = 1f;
    public float PitchThreshold = 2f;

    private float speed;
    private float oldPitch;
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
            speed = Mathf.Clamp(s, MinSpeed, MaxSpeed);
        }
            
        // Compute camera new position and smooth translate
        Vector3 new_pos = transform.position + new Vector3(speed, 0f, 0f);
        //float smooth = Mathf.SmoothDamp(speed);
        transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime * LerpTime);

        oldPitch = pitch;
    }
}
