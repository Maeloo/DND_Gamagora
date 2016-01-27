using UnityEngine;
using System.Collections;

public class Jinjo : MonoBehaviour
{
    private Animator anim;
    public int Id { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("End", false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            player.SetJinjo(this);

            if (anim != null)
            {
                anim.SetBool("End", true);
            }
        }
    }

    public void SetColorNumber(int number)
    {
        Id = number;
        if (anim != null)
        {
            anim.SetInteger("Color", number);
        }
    }
}
