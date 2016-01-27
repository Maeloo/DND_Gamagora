using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        Init();
    }

    public void Init()
    {
        if (anim != null)
            anim.SetBool("Check", false);
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            if(!anim.GetBool("Check"))
            {
                player.SetCheckpoint(transform.position);

                if (anim != null)
                {
                    anim.SetBool("Check", true);
                }
            }      
        }
    }
}
