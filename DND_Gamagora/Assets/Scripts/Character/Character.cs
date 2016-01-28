using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int life = 4;
    [SerializeField]
    private float stamina = 100;
    [SerializeField]
    private float special = 100;
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
    private int nbJump = 1;
    private int nbCurrentJump = 0;
    private bool doublejump = true;
    [SerializeField]
    private LayerMask WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField]
    private LayerMask WhatIsPlatform;
    [SerializeField]
    private Bullet kamehameha;
    [SerializeField]
    private Bullet special_tornado;

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    private Transform ceilingCheck;   // A position marking where to check for ceilings
    const float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator anim;            // Reference to the player's animator component.
    private Rigidbody2D rb;
    private bool facingRight = true;  // For determining which way the player is currently facing.
    internal Vector3 lastCheckpointPos;
    internal float lastCheckpointMusicTime;

    //List Collider Slide and Run
    public BoxCollider2D RunBox;
    public CircleCollider2D RunCircle;
    public BoxCollider2D SlideBox;
    public CircleCollider2D SlideCircle;

    private int noteCount;
    public Text noteText;
    private float lastAttack;
    private bool falling;
    public float invulnerabilityTimeBonus = 3f;
    private CharacterCamera cam;

    private Pool<Bullet> kamehamehas;
    private Pool<Bullet> tornados;

    private Vector3 direction = Vector3.right;
    private AudioProcessor audio_process;
    private List<Jinjo> jinjos;

    private float _baseLife;
    private float _baseStamina;
    private float _baseSpecial;

    private bool _noStamina;


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

        tornados = new Pool<Bullet>(special_tornado, 4, 8);
        tornados.automaticReuseUnavailables = true;

        _baseLife = life;
        _baseStamina = stamina;
        _baseSpecial = special;

        cam = Camera.main.GetComponent<CharacterCamera>();
        audio_process = AudioProcessor.Instance;
        lastCheckpointMusicTime = audio_process.GetMusicCurrentTime();

        jinjos = new List<Jinjo>(6);
    }

    void Start()
    {
        //special = 0;
        //HUDManager.instance.setSpecial(special);
        GameManager.Instance.SetPause(true);

        HUDManager.Instance.startCooldown(this, 3f);
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


    private bool _slide;
    public void Move(float move, bool slide, bool jump, bool run)
    {
        // If crouching, check to see if the character can stand up
        if (!slide && anim.GetBool("Slide"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, WhatIsGround))
            {
                slide = true;
            }
        }

        if (_slide != slide)
        {
            if(!slide)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1.0f, Vector3.up);
                foreach(RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Column"))
                        slide = true;
                }
            }
            
            _slide = slide;

            SlideCollider(slide);
        }

        if (grounded)
            nbCurrentJump = 0;

        // Set whether or not the character is crouching in the animator
        anim.SetBool("Slide", slide);
        //anim.SetBool("Run", !crouch && run);

        //only control the player if grounded or airControl is turned on
        if (grounded || (AirControl && !Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, WhatIsPlatform)))
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            if (slide)
            {
                move *= CrouchSpeed;
            }                
            else if (run && !_noStamina)
            {
                stamina = stamina < 2 ? 0 : stamina - 6;

                HUDManager.Instance.setStamina(stamina / _baseStamina);

                if (stamina == 0)
                {
                    _noStamina = true;
                }

                move *= RunSpeed;
            }                

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
        if (nbCurrentJump < nbJump && jump)
        {
            if (doublejump)
                StartCoroutine(jumping(run));
        }
        if (IsFalled() && !falling)
        {
            falling = true;

            anim.SetBool("Fall", true);
            GameManager.Instance.SetPause(true);

            if (cam != null)
            {
                cam.ResetCamToCheckPoint(lastCheckpointPos);
            }

            audio_process.RewindSound(lastCheckpointMusicTime);

            Invoke("MoveToLastCheckPoint", 1.0f);
        }            
    }

    private IEnumerator jumping(bool run)
    {
        doublejump = false;
        nbCurrentJump++;
        // Add a vertical force to the player.
        grounded = false;
        anim.SetBool("Ground", false);
        float str = JumpForce;
        if (run)
            str *= RunSpeed;
        rb.velocity=(new Vector2(0,0));
        rb.AddForce(new Vector2(0f, JumpForce));
        yield return new WaitForSeconds(.1f);
        doublejump = true;
    }


    void Update()
    {
        stamina = stamina < _baseStamina ? stamina + 1.0f : _baseStamina;
        HUDManager.Instance.setStamina(stamina / _baseStamina);

        if (_noStamina && stamina >= 50.0f )
        {
            _noStamina = false;
        }

        special = special < _baseSpecial ? special + .1f : _baseStamina;
        HUDManager.Instance.setSpecial(special / _baseSpecial);
    }


    public void SlideCollider(bool slide)
    {
        if (slide)
        {
            RunBox.enabled = false;
            RunCircle.enabled = false;

            SlideBox.enabled = true;
            SlideCircle.enabled = true;
        }
        else
        {
            RunBox.enabled = true;
            RunCircle.enabled = true;

            SlideBox.enabled = false;
            SlideCircle.enabled = false;
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


    public void Special()
    {
        if(special == _baseSpecial)
        {
            anim.SetTrigger("Attack");

            special = 0;
            HUDManager.Instance.setSpecial(special);

            Bullet b;
            if (tornados.GetAvailable(false, out b))
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
        if (TerrainManager.Instance._lastPos.x < TerrainManager.Instance.firstPlatform.position.x)
            TerrainManager.Instance._lastPos.x = TerrainManager.Instance.firstPlatform.position.x;
        TerrainManager.Instance.SpawnPlatform(Type_Platform.Classic);
        TerrainManager.Instance.SpawnPlatform(Type_Platform.Classic);

        Hit(1);
        transform.position = lastCheckpointPos;
        
        falling = false;
        HUDManager.Instance.startCooldown(this, 3f);
    }

    public void EndCooldownCheckpoint()
    {
        GameManager.Instance.SetPause(false);
        audio_process.PlayMusic();
    }

    public void SetCheckpoint(Vector3 checkpoint_pos)
    {
        lastCheckpointPos = checkpoint_pos;   
        lastCheckpointMusicTime = audio_process.GetMusicCurrentTime();
    }

    public void SetJinjo(Jinjo jinjo)
    {
        jinjos.Add(jinjo);
        HUDManager.Instance.setJinjo(jinjo);
        TerrainManager.Instance.DeleteJinjo(jinjo);
    }

    public void Hit(int power)
    {
        if (isInvincible)
            return;

        life -= power;
        life = life < 0 ? 0 : life;

        HUDManager.Instance.setLife(life == 0 ? 0 : (float)life / _baseLife);

        if (!dead && life == 0)
            GameOver();

        StartCoroutine(startInvulnerability());
    }


    bool dead;
    void GameOver()
    {
        dead = true;
        GameManager.Instance.SetPause(true);
        HUDManager.Instance.showGameOver();
    }

    public void AddNote()
    {
        ++noteCount;
        noteText.text = " x " + noteCount;
    }

    public void StartInvulnerabilityCoroutine()
    {
        StartCoroutine(startInvulnerability(invulnerabilityTimeBonus, true));
    }

    [SerializeField]
    GameObject shield;

    private bool isInvincible;
    IEnumerator startInvulnerability(float time = 1.0f, bool activeShield = false)
    {
        isInvincible = true;

        shield.SetActive(activeShield);

        Collider2D[] cs = GetComponents<Collider2D>();

        foreach(Collider2D c in cs)
        {
            if (c.isTrigger)
                c.enabled = false;
        }

        if(activeShield)
        {
            yield return new WaitForSeconds(time);
        } else
        {
            for (int i = 0; i < time / 1.8f; ++i)
            {

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
            }
        }

        SlideCollider(_slide);

        shield.SetActive(false);

        isInvincible = false;
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

