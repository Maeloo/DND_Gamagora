using UnityEngine;
using System.Collections;

public class Invincibility : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            ScoreManager.Instance.AddPoint(100);

            player.StartInvulnerabilityCoroutine();

            GetComponent<Animator>().SetBool("isPopping", true);
            Invoke("CallRelease", 0.5f);
        }
    }

    void CallRelease()
    {

        GetComponent<Animator>().SetBool("isPopping", false);
        GetComponentInParent<Bonus>().Release();
    }
}
