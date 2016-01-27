using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsPause { get; private set; }

    // Guarantee this will be always a singleton only - can't use the constructor!
    protected InputManager() { }

    // Use this for initialization
    void Awake()
    {
	    
	}
	
	// Update is called once per frame
	void Update()
    {
        IsRunning = Input.GetButton("Run");
        IsJumping = Input.GetButtonDown("Jump");
        IsSliding = Input.GetButton("Slide");

        if(Input.GetButtonDown("Cancel"))
        {
            IsPause = !IsPause;
            GameManager.Instance.SetPause(IsPause);
        }
    }

    public float GetHorizontalAxis()
    {
        return Input.GetAxis("Horizontal");
    }
}
