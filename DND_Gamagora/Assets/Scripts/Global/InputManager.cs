using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }

    public bool IsMoveDown { get; private set; }
    public bool IsMoveUp { get; private set; }
    public bool IsMoveRight { get; private set; }
    public bool IsMoveLeft { get; private set; }

    // Guarantee this will be always a singleton only - can't use the constructor!
    protected InputManager() { }

    // Use this for initialization
    void Awake()
    {
	    
	}
	
	// Update is called once per frame
	void Update()
    {
        IsRunning = Input.GetKey(Constants.RunButton);
        IsJumping = Input.GetKeyDown(Constants.JumpButton);

        IsMoveDown = Input.GetKey(Constants.CrouchButton);
        IsMoveUp = Input.GetKey(Constants.MoveUpButton);
        IsMoveRight = Input.GetKey(Constants.MoveRightButton);
        IsMoveLeft = Input.GetKey(Constants.MoveLeftButton);
    }
}
