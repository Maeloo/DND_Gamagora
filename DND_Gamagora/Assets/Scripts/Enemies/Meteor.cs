using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

    public void spawn(Vector3 position)
    {
        transform.position = position;

        float rand = Random.Range(.5f, 1.0f);
        transform.localScale = new Vector3(rand, rand, rand);

        GetComponent<Rigidbody2D>().isKinematic = false;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Platform pf = col.collider.GetComponentInParent<Platform>();

        if(pf != null)
        {
            pf.makeFall();
        }
    }


    void Update()
    {
        if(transform.position.y < -100)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Enemy>().Release();
        }
    }
}
