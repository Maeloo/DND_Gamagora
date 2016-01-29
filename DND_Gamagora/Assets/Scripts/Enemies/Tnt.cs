using UnityEngine;
using System.Collections;

public class Tnt : MonoBehaviour
{
    public Sprite _sprite;

    public void Init()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.layer == 11)
            {
                child.gameObject.GetComponent<Animator>().SetTrigger("Spawn");
                child.gameObject.GetComponent<BoxCollider2D>().enabled=true;
            }
        }
    }

    public void spawn(Vector3 position)
    {
        transform.position = position;
    }
}