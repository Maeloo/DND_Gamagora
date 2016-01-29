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

    public Game.Type_Bullet type;
    public float damage;
    public float speed;
    public GameObject specialFX;

    public bool Copy2D;

    protected bool shooted;

    protected Vector3 scale;

    protected Vector3 _direction;

    protected float timeDestroyInit = 5f;
    protected float timeDestroy;


    public Bullet Create()
    {
        return SCreate();
    }

    public void Duplicate(Bullet a_template)
    {
        type = a_template.type;
        specialFX = a_template.specialFX;

        this.gameObject.layer = a_template.gameObject.layer;

        foreach (Transform child in a_template.transform)
        {
            GameObject obj = Instantiate<GameObject>(child.gameObject);
            obj.transform.SetParent(transform);

            obj.transform.localPosition = child.localPosition;
            obj.transform.localRotation = child.localRotation;
            obj.transform.localScale = child.localScale;
            obj.gameObject.layer = child.gameObject.layer;
        }

        if(a_template.Copy2D)
        {
            gameObject.AddComponent<SpriteRenderer>().sprite = a_template.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<SpriteRenderer>().flipX = a_template.GetComponent<SpriteRenderer>().flipX;
            gameObject.GetComponent<SpriteRenderer>().color = a_template.GetComponent<SpriteRenderer>().color;

            gameObject.AddComponent<PolygonCollider2D>().isTrigger = a_template.GetComponent<Collider2D>().isTrigger;
            gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
        }     
        
        if(type == Game.Type_Bullet.Special)
        {
            BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
            box.isTrigger = true;
            box.size = new Vector2(1.0f, 5.0f);
            box.offset = new Vector2(.0f, 1.0f);

            Rigidbody2D body = gameObject.AddComponent<Rigidbody2D>();
            body.useAutoMass = true;
            body.freezeRotation = true;
            body.isKinematic = true;
        }

        scale = a_template.transform.localScale;

        damage = a_template.GetComponent<Bullet>().damage;
        speed = a_template.GetComponent<Bullet>().speed;

        transform.position = new Vector3(9999f, 9999f, 9999f);
        this.gameObject.SetActive(false);
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

        if(type != Game.Type_Bullet.Special)
        {
            this.gameObject.SetActive(false);
        }        

        _pool.onRelease(this);
    }

    public void shoot(Vector3 position, Vector3 direction)
    {

        if(type != Game.Type_Bullet.Special)
        {
            transform.localScale = Vector3.zero;
            
            iTween.ScaleTo(gameObject, scale, .3f);   
        }

        transform.position = position;
        
        _direction = direction;

        transform.right = _direction;

        shooted = true;
        timeDestroy = timeDestroyInit;
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if(timeDestroy < 0)
        {
            shooted = false;
            Release();
        }
        else if(shooted)
        {
            Vector3 newPosition = transform.position + speed * _direction;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
            timeDestroy -= Time.deltaTime;
        }        
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(type == Game.Type_Bullet.Enemy)
        {
            if (Game.Data.ACCESSIBILITY_MODE)
                return;

            Character player = col.GetComponent<Character>();

            if (player != null)
            {
                player.Hit(1);

                Release();
            }
        }

        if (type == Game.Type_Bullet.Player)
        {
            Enemy e = col.GetComponent<Enemy>();
            Enemy e2 = col.GetComponentInParent<Enemy>();

            if (e != null && e.type == Game.Type_Enemy.Shooter)
            {
                e.onHit();

                Release();

                ScoreManager.Instance.AddPoint((int)(damage + 10 * (UnityEngine.Random.value)));
            }

            if (e2 != null && e2.type == Game.Type_Enemy.Tnt && !col.CompareTag("Column"))
            {
                e2.onHit();
                
                Release();

                ScoreManager.Instance.AddPoint((int)(damage + 10 * (UnityEngine.Random.value)));
            }
        }

        if (type == Game.Type_Bullet.Special)
        {
            Enemy e = col.GetComponent<Enemy>();
            Enemy e2 = col.GetComponentInParent<Enemy>();
            Bullet b = col.GetComponent<Bullet>();

            if (e != null && e.type == Game.Type_Enemy.Shooter)
            {
                e.onHit();

                ScoreManager.Instance.AddPoint((int)(damage + 10 * (UnityEngine.Random.value)));
            }

            if (e2 != null && e2.type == Game.Type_Enemy.Tnt && !col.CompareTag("Column"))
            {
                e2.onHit();
                
                ScoreManager.Instance.AddPoint((int)(damage + 10 * (UnityEngine.Random.value)));
            }

            if(b != null && b.type == Game.Type_Bullet.Enemy)
            {
                Instantiate(specialFX, b.transform.position, Quaternion.identity);
                b.Release();                
            }
        }
    }

}
