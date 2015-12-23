using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour, Poolable<Platform> {

    static int num = 0;

    protected static Platform SCreate ( ) {
        GameObject obj = new GameObject ( );
        obj.name = "platform_" + num.ToString ( "000" );
        obj.transform.SetParent ( TerrainManager.Instance.transform );

        Platform self = obj.AddComponent<Platform> ( );

        ++num;

        return self;
    }

    protected Pool<Platform> _pool;


    public Platform Create ( ) {
        return Platform.SCreate ( );
    }


    public void Register ( UnityEngine.Object pool ) {
        _pool = pool as Pool<Platform>;
    }


    public bool IsReady ( ) {
        return !GetComponent<Renderer>().IsVisibleFrom(Camera.main);
    }


    public void Duplicate ( Platform a_template ) {
        foreach(Transform child in a_template.transform)
        {
            GameObject obj = Instantiate(child.gameObject) as GameObject;
            obj.transform.SetParent(transform);
        }

        transform.position = new Vector3(1000, 1000, 3);
    }


    public void Release ( ) {
        _pool.onRelease ( this );
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }


    void Update()
    {
        if (Camera.main.transform.position.x - transform.position.x > Screen.width)
            Release();
    }

}
