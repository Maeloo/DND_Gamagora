using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour
{
    // Move speed 
    [SerializeField]
    public float LerpTime;

    //[SerializeField]
    //public float MinSpeed;
    //[SerializeField]
    //public float MaxSpeed;
    //[SerializeField]
    //public float PitchInfluence;
    //[SerializeField]
    //public float PitchThreshold;
    //[SerializeField]
    //public float SmoothTime;
    //[SerializeField]
    //public float DeltaTime;
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 0f);

    private Transform target;

    //private float speed;
    //private float oldPitch;
    //private float currentVelocity;
    //private AudioProcessor audioProcess;
    private Vector3 target_pos;
    private bool targeting;

	// Use this for initialization
	void Awake()
    {
        //audioProcess = AudioProcessor.Instance;
        //speed = MinSpeed;
        //oldPitch = audioProcess.PitchValue;
        target = LoadCharacter.Instance.GetCharacter().transform;
        target_pos = target.position;
        targeting = false;
        transform.position = transform.position + offset;
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        // === For later use ===

        // Change speed over pitch
        //float pitch = audioProcess.PitchValue;
        //float delta = pitch - oldPitch;

        //if (Mathf.Abs(delta) > PitchThreshold)
        //{
        //    float s = speed + delta * PitchInfluence;
        //    speed = Mathf.SmoothDamp(speed, Mathf.Clamp(s, MinSpeed, MaxSpeed), ref currentVelocity, SmoothTime, MaxSpeed, Time.deltaTime * DeltaTime);
        //}

        //// Compute camera new position and smooth translate
        //Vector3 new_pos = transform.position + new Vector3(speed, 0f, 0f);
        //float smooth = Mathf.SmoothDamp(speed);

        if (!targeting)
            target_pos = new Vector3(target.position.x, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, target_pos, Time.deltaTime * LerpTime);

        if(targeting)
        {
            if (Vector3.Distance(transform.position, target_pos) < 0.5f)
                targeting = false;
        }

        //oldPitch = pitch;
    }

    public void ResetCamToCheckPoint(Vector3 pos)
    {
        target_pos = new Vector3(pos.x, transform.position.y, transform.position.z);
        targeting = true;
    }
}
