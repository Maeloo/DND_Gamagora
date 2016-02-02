using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager>
{
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsAttacking { get; private set; }

    public bool IsSpecial { get; private set; }
    // UI 
    public bool IsUp { get; private set; }
    public bool IsDown { get; private set; }
    public bool IsValidate { get; private set; }
    public bool IsNext { get; private set; }
    public bool IsCancel { get; private set; }
    // Guarantee this will be always a singleton only - can't use the constructor!
    protected InputManager() { }

    public float deadZone = 0.5f;
    public float sensitivity = 1.5f;

    // Update is called once per frame
    void Update()
    {
        IsRunning = Input.GetButton("Run");
        IsJumping = Input.GetButtonDown("Jump");
        IsSliding = Input.GetButton("Slide");

        if(Input.GetButtonDown("Cancel") && GameManager.Instance.IsMainScene() &&
            ((GameManager.Instance.Pause && GameManager.Instance.PauseHUD) || 
            (!GameManager.Instance.Pause && !GameManager.Instance.PauseHUD)))
        {
            GameManager.Instance.SetPauseHUD(!GameManager.Instance.Pause);
        }

        IsAttacking = Input.GetButton("Attack");
        IsSpecial = Input.GetButton("Special");

        /********** UI *********/
        IsUp = Input.GetAxis("Vertical") > 0f;
        IsDown = Input.GetAxis("Vertical") < 0f; ;
        IsNext = Input.GetButtonDown("Attack");
        IsValidate = Input.GetButtonDown("Cancel");
        IsCancel = Input.GetButtonDown("Special");

        /**** Accessibilité ****/
        if (Game.Data.ACCESSIBILITY_MODE)
        {

            Vector2 currentMousePosition = Input.mousePosition;
            // Calculer les differences de position entre deux frames
            Vector2 deltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (Input.touchCount > 0)
            {
                deltaPosition = Input.touches[0].deltaPosition * sensitivity;
            }

            // TODO Sauter 
            // DeltaPosition est en haut
            //if (deltaPosition.y > deadZone)
            if (currentMousePosition.y > 2.0f * Screen.height / 3.0f)
            {
                Debug.Log("jump");
                //m.Jump();
                IsJumping = true;
            } else
            {
                IsJumping = false;
            }

            // TODO Glisser
            // DeltaPosition est en bas
            //if (deltaPosition.y < - deadZone)
            if (currentMousePosition.y < Screen.height / 3.0f)
            {
                Debug.Log("slide");
                //m.Slide();
                IsSliding = true;
            } else
            {
                IsSliding = false;
            }

            // TODO Accelerer
            // DeltaPosition est en droite
            //if (deltaPosition.x > deadZone)
            if (currentMousePosition.x > 2.0f * Screen.width / 3.0f)
            {
                Debug.Log("sprint");
                //m.Sprint();
                IsRunning = true;
            } else
            {
                IsRunning = false;
            }

            // TODO Freinner
            // DeltaPosition est en gauche
            if (deltaPosition.x < - deadZone)
            {
                Debug.Log("slow");
                //m.SlowDown();
            }

            if(Input.GetMouseButtonDown(0))
            {
                IsAttacking = true;
            } else
            {
                IsAttacking = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                IsSpecial = true;
            }
            else
            {
                IsSpecial = false;
            }
        }
    }

    public float GetHorizontalAxis()
    {
        return Game.Data.ACCESSIBILITY_MODE ? .5f : Input.GetAxis("Horizontal");
    }
}
