using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour, Poolable<Enemy>
{

    static int num = 0;

    protected static Enemy SCreate()
    {
        GameObject obj = new GameObject();
        obj.name = "enemy_" + num.ToString("000");
        obj.transform.SetParent(EnemyManager.Instance.transform);

        Enemy self = obj.AddComponent<Enemy>();

        ++num;

        return self;
    }

    protected Pool<Enemy> _pool;

    public Game.Type_Enemy type;
    public bool Copy3D;


    public Enemy Create()
    {
        return Enemy.SCreate();
    }


    public void Register(UnityEngine.Object pool)
    {
        _pool = pool as Pool<Enemy>;
    }


    public bool IsReady()
    {
        throw new NotImplementedException();
    }


    public void Duplicate(Enemy a_template)
    {
        type = a_template.type;

        foreach (Transform child in a_template.transform)
        {
            GameObject obj = Instantiate<GameObject>(child.gameObject);
            obj.transform.SetParent(transform);

            obj.transform.localPosition = child.localPosition;
            obj.transform.localRotation = child.localRotation;
            obj.transform.localScale = child.localScale;
        }

        if (a_template.Copy3D)
        {
            gameObject.AddComponent<MeshFilter>().mesh = a_template.GetComponent<MeshFilter>().mesh;
            gameObject.AddComponent<MeshRenderer>().materials = a_template.GetComponent<MeshRenderer>().materials;  
        }

        switch(type) 
        {
            case Game.Type_Enemy.CrazyFireball:
                gameObject.AddComponent<ExplosionMat>();
                gameObject.AddComponent<CrazyFireball>();
                break;

            case Game.Type_Enemy.Shooter:
                gameObject.AddComponent<Shooter>().bulletPrefab = a_template.GetComponent<Shooter>().bulletPrefab;
                break;
        }
    }


    public void Release()
    {
        _pool.onRelease(this);
    }


    public void spawn(Vector3 position, Transform player)
    {
        switch (type)
        {
            case Game.Type_Enemy.CrazyFireball:
                gameObject.GetComponent<CrazyFireball>().spawn(position);
                break;

            case Game.Type_Enemy.Shooter:
                gameObject.GetComponent<Shooter>().init(position, player);
                break;
        }
    }


    public void shoot()
    {
        gameObject.GetComponent<Shooter>().shoot();
    }
}
