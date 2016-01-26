using UnityEngine;
using System.Collections;

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
    public bool _3D;


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
        return !GetComponent<Renderer>().IsVisibleFrom(Camera.main);
    }


    public void Duplicate(Enemy a_template)
    {
        if (a_template._3D)
        {
            gameObject.AddComponent<MeshFilter>().mesh = a_template.GetComponent<MeshFilter>().mesh;
            gameObject.AddComponent<MeshRenderer>().materials = a_template.GetComponent<MeshRenderer>().materials;

            if (type == Game.Type_Enemy.CrazyFireball)
            {
                gameObject.AddComponent<ExplosionMat>();
                gameObject.AddComponent<CrazyFireball>();
            }
        }
    }


    public void Release()
    {
        _pool.onRelease(this);
    }


    public void spawn(Vector3 position)
    {
        if (type == Game.Type_Enemy.CrazyFireball)
        {
            gameObject.GetComponent<CrazyFireball>().spawn(position);
        }
    }
}
