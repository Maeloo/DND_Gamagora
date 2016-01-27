using UnityEngine;
using System.Collections;

public class Invincibility : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            player.StartInvulnerabilityCoroutine();

            GetComponent<Animator>().SetTrigger("Pop");
            Invoke("CallRelease", 0.5f);
        }
    }

    void CallRelease()
    {
        GetComponent<Animator>().SetTrigger("BackToIdle");
        GetComponentInParent<Bonus>().Release();
    }
}
