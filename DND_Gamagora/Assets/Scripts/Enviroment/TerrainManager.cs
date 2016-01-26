using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;
    [SerializeField] Platform HightPlatform;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Checkpoint;
    [SerializeField] int platformCountBeforeCheckpoint = 100;
    [SerializeField] float offsetYCheckpoint = 1f;
    internal float offsetXPlatform;
    protected TerrainManager ( ) { }

    internal float classic_width;

    protected Dictionary<Type_Platform, Pool<Platform>> pools;

    public Transform firstPlatform;
    public float spawnTime = .2f;

    private float _lastSpawn;
    internal Vector3 _lastPos;
    private int platformCount = 0;
    void Awake()
    {
        Checkpoint.transform.position = new Vector3(-1000, -1000, -1000);
        pools = new Dictionary<Type_Platform, Pool<Platform>>();

        Pool<Platform> classicPool = new Pool<Platform>(ClassicPlatform, 64, 128);
        classicPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Classic, classicPool);        

        Pool<Platform> bouncyPool = new Pool<Platform>(BouncyPlatform, 8, 16);
        bouncyPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Bouncy, bouncyPool);

        Pool<Platform> hightPlatformPool = new Pool<Platform>(HightPlatform, 64, 128);
        hightPlatformPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Hight, hightPlatformPool);

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
            if (platformCount > platformCountBeforeCheckpoint)
            {
                Checkpoint.transform.position = _lastPos + new Vector3(0f, offsetYCheckpoint, 0f);
                platformCount = 0;
            }
        }
    }


    // Offset à 0 = platform actuelle, Offset à 3 = 3 platform en avance .. etc
    bool getClassicPlatform(ref Platform platform, int offset = 0)
    {
        for (int i = 0; i < pools[Type_Platform.Classic].usedObjects.Count; i++)
        {
            Vector3 pos_pf = pools[Type_Platform.Classic].usedObjects[i].transform.position;
            if (Player.transform.position.x + offset * classic_width > pos_pf.x &&
                Player.transform.position.x + offset * classic_width < pos_pf.x + classic_width)
            {
                Vector3 pos_pf_previous = pools[Type_Platform.Classic].usedObjects[i - 1].transform.position;
                Vector3 pos_pf_next = pools[Type_Platform.Classic].usedObjects[i + 1].transform.position;
                if (!CheckHightPlatform(pos_pf, pos_pf_previous, pos_pf_next, offset))
                {
                    platform = pools[Type_Platform.Classic].usedObjects[i];
                    return true;
                }
            }
        }

        return false;
    }


    bool CheckHightPlatform(Vector3 pf, Vector3 pf_previous, Vector3 pf_next, int offset)
    {
        for (int i = 0; i < pools[Type_Platform.Hight].usedObjects.Count; i++)
        {
            Vector3 pos_pf = pools[Type_Platform.Hight].usedObjects[i].transform.position;
            if (pos_pf.x == pf.x || pos_pf.x == pf_previous.x || pos_pf.x == pf_next.x)
            {
                return true;
            }
        }
        return false;
    }

    internal void SpawnPlatform(Type_Platform type)
    {
        Platform pf;

        if(pools[type].GetAvailable(false, out pf))
        {
            Vector3 pos = _lastPos;
            pos.x += classic_width;

            pf.SetPosition(pos);

            _lastPos = pos;
        }
        ++platformCount;
    }

    internal void SpawnPlatformHight(Type_Platform type)
    {
        Platform pf;

        if (pools[type].GetAvailable(false, out pf))
        {
            Vector3 pos = _lastPos;
            pos.x += classic_width;
            pos.y +=2.41f;
            pf.SetPosition(pos);
        }
    }


    public void ErasePlatform()
    {
        for (int i = pools[Type_Platform.Classic].usedObjects.Count-1 ; i >= 0; i--)
        {
            pools[Type_Platform.Classic].usedObjects[i].Release();
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
