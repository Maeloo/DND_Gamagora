using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;


    protected TerrainManager ( ) { }

    protected float classic_width;
    protected float last_cam_x;

    protected Dictionary<Type_Platform, Pool<Platform>> pools;


    void Awake()
    {
        pools = new Dictionary<Type_Platform, Pool<Platform>>();

        pools.Add(Type_Platform.Classic, new Pool<Platform>(ClassicPlatform, 10, 20));
        pools.Add(Type_Platform.Bouncy, new Pool<Platform>(BouncyPlatform, 5, 10));

        classic_width = ClassicPlatform.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        last_cam_x = Camera.main.transform.position.x;
    }


    void Update()
    {
        if(Camera.main.transform.position.x - last_cam_x > classic_width)
        {
            SpawnPlatform(Type_Platform.Classic);

            last_cam_x = Camera.main.transform.position.x;
        }
    }


    void SpawnPlatform(Type_Platform type)
    {
        Platform pf;

        if(pools[type].GetAvailable(true, out pf))
        {
            Vector3 tar = new Vector3(Screen.width, .0f, .0f);
            Vector3 pos = Camera.main.ScreenToWorldPoint(tar);
            pos.z =  .0f;
            pos.y = -3.0f;

            pf.SetPosition(pos);
        }
    }

}
