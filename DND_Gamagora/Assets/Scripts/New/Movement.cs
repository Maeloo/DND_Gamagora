using UnityEngine;
using System.Collections;
using System;
/*
• Sauter : button Jump
• Sprint : boutton Sprint 
• Glisser : boutton Sprint + flèche du bas 
• S’accrocher : Automatique quand saut sur un bord / un mur
• Saut de mur : boutton Jump quand accrocher
• Baisser : flèche du bas
*/

public class Movement : MonoBehaviour {
    enum gameState { walking, sprinting, slowing, jumping, ducking, sliding, hanging, overWall};
    gameState currentState;

    KeyCode SprintButton = KeyCode.RightArrow;
    KeyCode JumpButton = KeyCode.UpArrow;
    KeyCode SlowDownButton = KeyCode.LeftArrow;
    KeyCode DuckButton= KeyCode.DownArrow;

    public Vector2 finish;
    float speed;
    float acceleration;
    float sprintAcceleration= 2f;
    float slowDownAcceleration=0.5f;
    float jumpHeight=500;
 
    GameObject upperBody;
    GameObject lowerBody;

    void Awake ( ) {
        upperBody = transform.GetChild(0).gameObject;
        
        finish = transform.position;
        DefState();//start with the default state
        Debug.Log(upperBody.transform.position.y);
        Debug.Log(transform.position.y);
        Debug.Log(upperBody.transform.position.y- transform.position.y);
    }
    void DefState()
    {
        speed = 1f; //choix convenable à faire
        acceleration = 1f; //vitesse normale
        currentState = gameState.walking;
    }
    void Update ( ) {
        Move();
        ApplyGravity();
        MovementLogic();
        if ( Input.GetKey ( SlowDownButton ) ) {
            SlowDown();
        }
        else if ( Input.GetKey ( SprintButton ) && !Input.GetKey ( DuckButton ) ) {
            Sprint();
        }
        else if ( Input.GetKeyDown ( JumpButton ) ) {
            if(currentState!=gameState.jumping)
            {
                currentState = gameState.jumping;
                Jump();
                Invoke("DefState", 0.4f);
            }
            
        }/*
        else if ( Input.GetKey ( DuckButton ) && !Input.GetKey ( SprintButton ) ) {
                Duck();
        }*/
        //else if ( Input.GetKey ( DuckButton ) && Input.GetKey ( SprintButton ) ) {
        else if (Input.GetKey(DuckButton))
        {
            if (currentState == gameState.walking)
            {
            Slide ();
            currentState = gameState.sliding;
            }
            
        }
        else
        {
            if (currentState == gameState.sprinting || currentState == gameState.slowing)
                DefState();
            if (currentState == gameState.ducking)
                Unduck();
        }
    }
 
    void MovementLogic()
    {
      
        if (currentState == gameState.walking)
        {
//            DefState();

        }
    }


    public void Move()
    {
        finish.x+=0.1f * acceleration;
        transform.position = Vector2.MoveTowards(transform.position, finish, speed);
    }
    public void SlowDown()
    {

        acceleration = Mathf.Lerp(acceleration, slowDownAcceleration, 1f);
    }

    public void Sprint()
    {
        acceleration = Mathf.Lerp(acceleration, sprintAcceleration,1f);
    }

    public void Jump() 
    {   
        finish.y=Mathf.Lerp(finish.y, jumpHeight,0.02f);
        finish.x += 0.4f * acceleration;
        
    }

    void ApplyGravity()
    {
        //        finish.y -= jumpHeight;
        finish.y = Mathf.Lerp(finish.y, -1, 0.2f);
    }
    public void Duck()
    {

        upperBody.transform.position = new Vector2(upperBody.transform.position.x, Mathf.Lerp(upperBody.transform.position.y, transform.position.y +0.25f, 1f));
    }
    public void Unduck()
    {

        upperBody.transform.position = new Vector2(upperBody.transform.position.x, Mathf.Lerp(upperBody.transform.position.y, transform.position.y + 1.1f, 1f));
    }

    public void Slide()
    {
        Quaternion rot = Quaternion.Euler(0, 0, 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f );
        acceleration = sprintAcceleration;
        Invoke("DefState", 0.5f);
        Invoke("StandUp", 0.6f);
    }
    void StandUp()
    {
        transform.rotation = Quaternion.identity;
        upperBody.transform.position = new Vector2(upperBody.transform.position.x, Mathf.Lerp(upperBody.transform.position.y, transform.position.y + 1.1f, 1f));

    }
}


