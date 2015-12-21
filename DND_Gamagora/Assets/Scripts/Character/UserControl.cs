using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
public class UserControl : MonoBehaviour
{
    private Character character;
    private bool jump;
    private InputManager inputs;

    private void Awake()
    {
        character = GetComponent<Character>();
        inputs = InputManager.Instance;
    }


    private void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = inputs.IsJumping;
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = inputs.IsCrunching;
        bool run = inputs.IsRunning;

        float h = inputs.GetHorizontalAxis(); 
        // Pass all parameters to the character control script.
        character.Move(h, crouch, jump, run);
        jump = false;
    }
}