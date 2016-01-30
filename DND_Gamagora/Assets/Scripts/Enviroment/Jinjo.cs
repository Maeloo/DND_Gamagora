using UnityEngine;
using System.Collections;


public class Jinjo : MonoBehaviour
{
    [SerializeField]
    private float AuraEmitDistance = 20f;

    private Animator anim;
    private ParticleSystem fireworks;
    private ParticleSystem halo;
    private bool triggered;
    private Transform player;
    private float timeBeforeLastAura;
    private ParticleSystem aura_particles;

    public int Id { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("End", false);
        triggered = false;
        player = LoadCharacter.Instance.GetCharacter().transform;
        timeBeforeLastAura = 0f;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < AuraEmitDistance && !triggered)
        {
            if(timeBeforeLastAura > 1f)
            {
                aura_particles.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z + 1.5f);
                aura_particles.gameObject.SetActive(true);
                aura_particles.Play(true);
                timeBeforeLastAura = 0f;
            }
        }

        timeBeforeLastAura += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!triggered)
        {
            Character player = col.gameObject.GetComponent<Character>();
            if (player != null)
            {
                if (aura_particles != null)
                    aura_particles.Stop(true);

                if (anim != null)
                {
                    anim.SetBool("End", true);
                }

                Invoke("DeleteJinjo", 1f);

                player.SetJinjo(this);
                ScoreManager.Instance.AddPoint(500);
                triggered = true;
            }
        }
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        HighPlatform hpf = col.GetComponent<HighPlatform>();

        if (hpf != null)
        {
            Vector3 temp = transform.position;
            temp.x--;
            transform.position = temp;
        }

        Enemy e = col.GetComponentInParent<Enemy>();

        if (e != null && e.type == Game.Type_Enemy.Tnt)
        {
            Vector3 temp = transform.position;
            temp.x++;
            transform.position = temp;
        }
    }

    private void DeleteJinjo()
    {
        Instantiate(fireworks, transform.position, transform.rotation);
    }

    public void SetColorNumber(int number)
    {
        Id = number;
        if (anim != null)
        {
            anim.SetInteger("Color", number);
        }
    }

    public void SetParticles(ParticleSystem boom, ParticleSystem aura)
    {
        if(boom != null)
        {
            fireworks = boom;
            fireworks.Stop();
        }

        if (aura != null)
        {
            halo = aura;
            aura_particles = (ParticleSystem)Instantiate(halo, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.rotation);
            aura_particles.Stop();
        }
    }
}
