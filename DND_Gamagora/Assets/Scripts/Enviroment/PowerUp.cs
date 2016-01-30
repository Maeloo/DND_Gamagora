using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    [SerializeField]
    GameObject FX;

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            Instantiate(FX, transform.position, Quaternion.identity);
            player.setUnlimiedStamina();
            GetComponentInParent<Bonus>().Release();

            Hashtable param = new Hashtable();
            param.Add("pitch", .7f);
            SceneAudioManager.Instance.playAudio(Game.Audio_Type.Bonus, param);
        }
    }
}
