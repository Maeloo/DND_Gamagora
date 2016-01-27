using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour, Poolable<Bonus> {

    static int num = 0;

    protected static Bonus SCreate()
    {
        GameObject obj = new GameObject();
        obj.name = "bonus_" + num.ToString("000");
        obj.transform.SetParent(BonusManager.Instance.transform);

        Bonus self = obj.AddComponent<Bonus>();

        ++num;

        return self;
    }

    protected Pool<Bonus> _pool;


    public Bonus Create()
    {
        return Bonus.SCreate();
    }


    public void Register(UnityEngine.Object pool)
    {
        _pool = pool as Pool<Bonus>;
    }


    public bool IsReady()
    {
        return !GetComponent<Renderer>().IsVisibleFrom(Camera.main);
    }


    public void Duplicate(Bonus a_template)
    {
        foreach (Transform child in a_template.transform)
        {
            GameObject obj = Instantiate(child.gameObject) as GameObject;
            obj.transform.SetParent(transform);
        }

        transform.position = new Vector3(1000, 1000, 3);
    }


    public void Release()
    {
        transform.position = new Vector3(-1000f, -1000f, -1000f);

        _pool.onRelease(this);
    }

    public void SetPosition(Vector3 position)
    {
        this.gameObject.SetActive(true);

        transform.position = position;
    }

}
