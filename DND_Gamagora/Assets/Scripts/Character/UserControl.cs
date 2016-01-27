using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
public class UserControl : MonoBehaviour
{
    private Character character;
    private bool jump;
    private bool slide;
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
        if (!slide)
        {
            // Read the jump input in Update so button presses aren't missed.
            slide = inputs.IsSliding;
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool run = inputs.IsRunning;
        bool attack = inputs.IsAttacking;

        float h = inputs.GetHorizontalAxis(); 
        
        // Pass all parameters to the character control script.
        character.Move(h, slide, jump, run);

        if (attack)
            character.Attack();

        jump = false;
        slide = false;
    }
}