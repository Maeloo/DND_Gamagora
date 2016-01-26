using UnityEngine;
using System.Collections;

public class Floaty : MonoBehaviour {

    public Vector2 offset;

    Vector3 initial_pos;

    public float force = .01f;

    void Awake()
    {
        initial_pos = transform.localPosition;
    }


    void Update()
    {
        Vector3 newpos = transform.localPosition;

        if (transform.localPosition.y > initial_pos.y + offset.x)
        {
            force = -Mathf.Abs(force);
        } else if (transform.localPosition.y < initial_pos.y - offset.y)
        {
            force = Mathf.Abs(force);
        }

        newpos.y += force;
        transform.localPosition = newpos;
    }
    
}
