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
        if (!GameManager.Instance.Pause)
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
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.Pause)
        {
            // Read the inputs.
            bool run = inputs.IsRunning;
            bool attack = inputs.IsAttacking;

            float h = inputs.GetHorizontalAxis();

            bool special = inputs.IsSpecial;


            // Pass all parameters to the character control script.
            character.Move(h, slide, jump, run);

            if (attack)
                character.Attack();

            if (special)
                character.Special();

            jump = false;
            slide = false;
        }
    }
}