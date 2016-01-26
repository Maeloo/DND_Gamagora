using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;
    [SerializeField] GameObject Player;


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

        Pool<Platform> classicPool = new Pool<Platform>(ClassicPlatform, 64, 128);
        classicPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Classic, classicPool);        

        Pool<Platform> bouncyPool = new Pool<Platform>(BouncyPlatform, 8, 16);
        bouncyPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Bouncy, bouncyPool);                

        classic_width = ClassicPlatform.GetComponentInChildren<SpriteRenderer>().bounds.size.x + 0.02f;

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


    // Offset à 0 = platform actuelle, Offset à 3 = 3 platform en avance .. etc
    bool getClassicPlatform(ref Platform platform, int offset = 0)
    {
        for (int i = 0; i < pools[0].usedObjects.Count; i++)
        {
            Vector3 pos_pf = pools[0].usedObjects[i].transform.position;
            if (Player.transform.position.x + offset * classic_width > pos_pf.x && 
                Player.transform.position.x + offset * classic_width < pos_pf.x + classic_width)
            {
                platform = pools[0].usedObjects[i];
                return true;
            }
        }

        return false;
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


    /******* Public methods ********/

    public bool makeCurrentClassicPlatformFall()
    {
        Platform pf = null;
        if(getClassicPlatform(ref pf, 2))
        {
            return pf.makeFall(10.0f, .3f);
        }

        return false;
    }

}
