using UnityEngine;
using System.Collections;
using Game;
public class Note : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            player.AddNote();
            BonusManager.Instance.RemoveBonus(transform.parent.gameObject.GetComponent<Bonus>(), Type_Bonus.Note);
            
        }
    }
}
