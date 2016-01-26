using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int life = 4;
    [SerializeField]
    private float yMin = -10f;
    [SerializeField]
    private float MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField]
    private float JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)]
    [SerializeField]
    private float CrouchSpeed = .36f;  // Amount of MaxSpeed applied to crouching movement. 1 = 100%
    [SerializeField]
    private float RunSpeed = 2f; // Multiplicateur de MaxSpeed
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private bool AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField]
    private Bullet kamehameha;
    public GameObject[] LifeUI;
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    private Transform ceilingCheck;   // A position marking where to check for ceilings
    const float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator anim;            // Reference to the player's animator component.
    private Rigidbody2D rb;
    private bool facingRight = true;  // For determining which way the player is currently facing.
    internal Vector3 lastCheckpointPos;

    private int noteCount;
    public Text noteText;
    private float lastAttack;
    private bool falling;

    private Pool<Bullet> kamehamehas;

    private Vector3 direction = Vector3.right;

    private void Awake()
    {
        noteCount = 0;
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lastCheckpointPos = transform.position;
        lastAttack = Time.time;
        kamehamehas = new Pool<Bullet>(kamehameha, 8, 16);
        kamehamehas.automaticReuseUnavailables = true;
    }


    private void FixedUpdate()
    {
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                grounded = true;
        }
        anim.SetBool("Ground", grounded);

        // Set the vertical animation
        anim.SetFloat("vSpeed", rb.velocity.y);
    }


    public void Move(float move, bool crouch, bool jump, bool run)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        anim.SetBool("Crouch", crouch);
        //anim.SetBool("Run", !crouch && run);

        //only control the player if grounded or airControl is turned on
        if (grounded || AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            if (crouch)
                move *= CrouchSpeed;
            else if (run)
                move *= RunSpeed;

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            rb.velocity = new Vector2(move * MaxSpeed, rb.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
                direction *= -1;
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
                direction *= -1;
            }
        }
        // If the player should jump...
        if (grounded && jump && anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            grounded = false;
            anim.SetBool("Ground", false);
            float str = JumpForce;
            if (run)
                str *= RunSpeed;
            rb.AddForce(new Vector2(0f, str));
        }
        if (IsFalled() && !falling)
        {
            falling = true;

            anim.SetBool("Fall", true);

            Invoke("MoveToLastCheckPoint", 1.0f);
            //MoveToLastCheckPoint();
        }            
    }

    public void Attack()
    {
        if(Time.time - lastAttack  > attackSpeed)
        {
            anim.SetTrigger("Attack");
            lastAttack = Time.time;

            Bullet b;
            if(kamehamehas.GetAvailable(false, out b))
            {
                b.shoot(transform.position, direction);
            }
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsFalled()
    {
        return transform.position.y < yMin;    
    }


    private void MoveToLastCheckPoint()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        anim.SetBool("Fall", false);
        
        TerrainManager.Instance.ErasePlatform();
        TerrainManager.Instance._lastPos = new Vector3(lastCheckpointPos.x - TerrainManager.Instance.classic_width, TerrainManager.Instance._lastPos.y, TerrainManager.Instance._lastPos.z);
        TerrainManager.Instance.SpawnPlatform(Type_Platform.Classic);

        Hit(1);
        transform.position = lastCheckpointPos;

        falling = false;
    }

    public void Hit(int power)
    {
        int oldLife = life;
        life -= power;
        life = life < 0 ? 0 : life;

        for(int i = life; i < oldLife; ++i)
        {
            LifeUI[i].SetActive(false);
        }

        //if(life <= 0)
        // Die();

        StartCoroutine(startInvulnerability());
    }

    public void AddNote()
    {
        ++noteCount;
        noteText.text = " x " + noteCount;
    }

    IEnumerator startInvulnerability(float time = 1.0f)
    {
        Collider2D[] cs = GetComponents<Collider2D>();

        foreach(Collider2D c in cs)
        {
            if (c.isTrigger)
                c.enabled = false;
        }

        fade(.3f, .35f);
        yield return new WaitForSeconds(.3f);
                                        
        fade(.3f, 1.0f);                 
        yield return new WaitForSeconds(.3f);
                                        
        fade(.3f, .35f);                  
        yield return new WaitForSeconds(.3f);
                                        
        fade(.3f, 1f);                   
        yield return new WaitForSeconds(.3f);
                                        
        fade(.3f, .35f);                 
        yield return new WaitForSeconds(.3f);
                                        
        fade(.3f, 1f);                  
        yield return new WaitForSeconds(.3f);

        foreach (Collider2D c in cs)
        {
            if (c.isTrigger)
                c.enabled = true;
        }
    }


    private void fade(float time, float alpha)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", time,
            "from", GetComponent<SpriteRenderer>().color.a,
            "to", alpha,
            "onupdate", "onAlphaChange"));
    }


    public void onAlphaChange(float value)
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = value;
        GetComponent<SpriteRenderer>().color = tmp;
    }
}

