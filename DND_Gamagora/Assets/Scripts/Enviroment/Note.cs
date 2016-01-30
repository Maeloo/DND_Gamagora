using UnityEngine;
using System.Collections;
using Game;

public class Note : MonoBehaviour {

    [SerializeField]
    GameObject FX;

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            Instantiate(FX, transform.position, Quaternion.identity);

            player.AddNote();
            GetComponentInParent<Bonus>().Release();

            ScoreManager.Instance.AddPoint(50);
        }
    }
}
