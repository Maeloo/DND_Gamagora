using UnityEngine;
using System.Collections;

public class CrazyFireball : MonoBehaviour
{

    void Start()
    {
        reset();
    }

    public void spawn(Vector3 position)
    {
        reset();

        transform.position = position;

        iTween.ScaleTo(gameObject, iTween.Hash(
            "time", .5f,
            "scale", new Vector3(3.0f, 3.0f, 3.0f),
            "easetype", iTween.EaseType.easeInOutExpo));
    }

    void reset ()
    {
        transform.localScale = Vector3.zero;
    }
    
}
