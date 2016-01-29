using UnityEngine;
using System.Collections;


public class Jinjo : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem fireworks;
    private bool triggered;

    public int Id { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("End", false);
        triggered = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!triggered)
        {
            Character player = col.gameObject.GetComponent<Character>();
            if (player != null)
            {
                if (anim != null)
                {
                    anim.SetBool("End", true);
                }
                Invoke("DeleteJinjo", 1.3f);

                player.SetJinjo(this);
                ScoreManager.Instance.AddPoint(500);
                triggered = true;
            }
        }
        
    }

    //void OnTriggerStay2D(Collider2D col)
    //{
    //    HighPlatform hpf = col.GetComponent<HighPlatform>();

    //    if (hpf != null)
    //    {
    //        Vector3 temp = transform.position;
    //        temp.x--;
    //        transform.position = temp;
    //    }

    //    Enemy e = col.GetComponentInParent<Enemy>();

    //    if (e != null && e.type == Game.Type_Enemy.Tnt)
    //    {
    //        Vector3 temp = transform.position;
    //        temp.x++;
    //        transform.position = temp;
    //    }
    //}

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

    public void SetParticles(ParticleSystem p)
    {
        if(p != null)
        {
            fireworks = p;
            fireworks.Stop();
        }
    }
}
