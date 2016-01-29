using UnityEngine;
using System.Collections;
using Game;
public class Heart : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            if (player.life < player.MaxLife) { 
                player.AddLife();
                GetComponentInParent<Bonus>().Release();
            }
        }
    }
}
