using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour, Poolable<Bullet> {

    static int num = 0;

    protected static Bullet SCreate()
    {
        GameObject obj = new GameObject();
        obj.name = "bullet_" + num.ToString("000");
        obj.transform.SetParent(GameObject.Find("Bullets").transform);

        Bullet self = obj.AddComponent<Bullet>();

        ++num;

        return self;
    }


    protected Pool<Bullet> _pool;

    public float damage;
    public float speed;

    protected bool shooted;

    protected Vector3 _direction;


    public Bullet Create()
    {
        return SCreate();
    }

    public void Duplicate(Bullet a_template)
    {

        gameObject.AddComponent<SpriteRenderer>().sprite = a_template.GetComponent<SpriteRenderer>().sprite;
        gameObject.GetComponent<SpriteRenderer>().flipX = a_template.GetComponent<SpriteRenderer>().flipX;

        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.AddComponent<Rigidbody2D>().isKinematic = true;

        damage = a_template.GetComponent<Bullet>().damage;
        speed = a_template.GetComponent<Bullet>().speed;

        transform.position = new Vector3(9999f, 9999f, 9999f);
    }

    public bool IsReady()
    {
        throw new NotImplementedException();
    }

    public void Register(UnityEngine.Object pool)
    {
        _pool = pool as Pool<Bullet>;
    }

    public void Release()
    {
        shooted = false;

        transform.position = new Vector3(9999f, 9999f, 3.0f);

        _pool.onRelease(this);
    }

    public void shoot(Vector3 position, Vector3 direction)
    {
        transform.localScale = Vector3.zero;
        transform.position = position;

        iTween.ScaleTo(gameObject, Vector3.one, .3f);

        _direction = direction;
        transform.right = _direction;

        shooted = true;
    }

    void Update()
    {
        if(shooted)
        {
            Vector3 newPosition = transform.position + speed * _direction;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
        }        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.GetComponent<Character>();

        if(player != null)
        {
            // Add damage to player

            Release();
        }
    }

}
