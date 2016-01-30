using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;

    private Animator anim;
    private ParticleSystem checkpoint_particles;

    void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject obj = (GameObject)Instantiate(particles, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        checkpoint_particles = obj.transform.FindChild("Particles").GetComponent<ParticleSystem>();

        Init();
    }

    public void Init()
    {
        if (anim != null)
        {
            anim.SetBool("Check", false);
        }
            
        if (checkpoint_particles != null)
        {
            checkpoint_particles.time = 0f;
            checkpoint_particles.Play(true);
            checkpoint_particles.transform.position = transform.position;
        }
    }

    public void ResetParticlesPos()
    {
        anim.SetBool("Check", true);
        if (checkpoint_particles != null)
            checkpoint_particles.Stop(true);
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            if(!anim.GetBool("Check"))
            {
                player.SetCheckpoint(transform.position);
                
                ScoreManager.Instance.AddPoint(100);

                if (anim != null)
                {
                    anim.SetBool("Check", true);
                }

                if(checkpoint_particles != null)
                    checkpoint_particles.Stop(true);
            }      
        }
    }
}
