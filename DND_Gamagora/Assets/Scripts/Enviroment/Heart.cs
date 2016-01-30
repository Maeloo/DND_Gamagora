using UnityEngine;
using System.Collections;
using Game;

public class Heart : MonoBehaviour {

    [SerializeField]
    GameObject FX;

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            if (player.life < player.MaxLife) {
                Instantiate(FX, transform.position, Quaternion.identity);
                player.AddLife();
                GetComponentInParent<Bonus>().Release();

                Hashtable param = new Hashtable();
                SceneAudioManager.Instance.playAudio(Audio_Type.Bonus, param);
            }
        }
    }
}
