using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;


    protected TerrainManager ( ) { }

    protected float classic_width;

    protected Dictionary<Type_Platform, Pool<Platform>> pools;

    public Transform firstPlatform;
    public float spawnTime = .2f;

    private float _lastSpawn;
    private Vector3 _lastPos;


    void Awake()
    {
        pools = new Dictionary<Type_Platform, Pool<Platform>>();

        pools.Add(Type_Platform.Classic, new Pool<Platform>(ClassicPlatform, 10, 12));
        pools.Add(Type_Platform.Bouncy, new Pool<Platform>(BouncyPlatform, 5, 10));

        pools[0].automaticReuseUnavailable = true;

        classic_width = ClassicPlatform.GetComponentInChildren<SpriteRenderer>().bounds.size.x;

        _lastPos = firstPlatform.position;

        _lastSpawn = Time.time;
    }


    void Update()
    {
        if (Time.time - _lastSpawn > spawnTime)
        {
            SpawnPlatform(Type_Platform.Classic);

            _lastSpawn = Time.time;
        }
    }


    void SpawnPlatform(Type_Platform type)
    {
        Platform pf;

        if(pools[type].GetAvailable(false, out pf))
        {
            Vector3 pos = _lastPos;
            pos.x += classic_width;

            pf.SetPosition(pos);

            _lastPos = pos;
        }
    }

}
