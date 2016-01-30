using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;
    [SerializeField] Platform HightPlatform;
    private GameObject Player;
    [SerializeField] GameObject Checkpoint;
    [SerializeField] int platformCountBeforeCheckpoint = 100;
    [SerializeField] float offsetYCheckpoint = 1f;
    [SerializeField]
    private GameObject base_jinjo;

    internal float offsetXPlatform;
    protected TerrainManager ( ) { }

    internal float classic_width;

    protected Dictionary<Type_Platform, Pool<Platform>> pools;

    public Transform firstPlatform;
    public float spawnTime = .2f;

    private float _lastSpawn;
    internal Vector3 _lastPos;
    private int platformCount = 0;

    private Dictionary<Jinjo, bool> jinjos;
    private Checkpoint CheckPointScript;
    private GameObject jinjo_to_delete;
    private Dictionary<string, ParticleSystem> jinjo_particles;
    private EnemyManager enemyManager;

    void Awake()
    {
        Player = LoadCharacter.Instance.GetCharacter();
        CheckPointScript = Checkpoint.GetComponent<Checkpoint>();
        Checkpoint.transform.position = new Vector3(-1000, -1000, -1000);
        pools = new Dictionary<Type_Platform, Pool<Platform>>();

        Pool<Platform> classicPool = new Pool<Platform>(ClassicPlatform, 64, 128);
        classicPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Classic, classicPool);        

        //Pool<Platform> bouncyPool = new Pool<Platform>(BouncyPlatform, 8, 16);
        //bouncyPool.automaticReuseUnavailables = true;
        //pools.Add(Type_Platform.Bouncy, bouncyPool);

        Pool<Platform> hightPlatformPool = new Pool<Platform>(HightPlatform, 64, 128);
        hightPlatformPool.automaticReuseUnavailables = true;
        pools.Add(Type_Platform.Hight, hightPlatformPool);

        classic_width = ClassicPlatform.GetComponentInChildren<SpriteRenderer>().bounds.size.x - 0.02f;

        _lastPos = firstPlatform.position;

        _lastSpawn = Time.time;

        ParticleSystem[] res = Resources.LoadAll<ParticleSystem>("Effects/");
        jinjo_particles = new Dictionary<string, ParticleSystem>(res.Length);
        for(int i = 0; i < res.Length; i++)
        {
            jinjo_particles.Add(res[i].name, res[i]);
        }
        
        InitJinjos();
    }

    void InitJinjos()
    {
        float delta = (GetTerrainSize() * 0.4f) / 6f;

        Camera cam = Camera.main;
        float vertExtent = cam.orthographicSize * 2f;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        // Calculations assume cam is position at the origin
        float minX = horzExtent - cam.transform.position.x * 0.5f;
        float maxX = cam.transform.position.x * 0.5f - horzExtent;
        float minY = vertExtent - cam.transform.position.y * 0.5f;
        float maxY = cam.transform.position.y * 0.5f - vertExtent;

        float height = (minY - maxY) * 0.5f;

        float max = cam.transform.position.y + height * 0.2f;
        float min = cam.transform.position.y - height * 0.2f;

        jinjos = new Dictionary<Jinjo, bool>(6);
        for (int i = 0; i < 6; i++)
        {
            float y = Random.Range(min, max);
            GameObject obj_jinjo = (GameObject)Instantiate(base_jinjo, new Vector3((i + 1) * delta, y, 0f), Quaternion.identity);
            Jinjo j = obj_jinjo.GetComponent<Jinjo>();

            j.SetColorNumber(i);

            if (i == 0)
                j.SetParticles(jinjo_particles["Jinjo_Violet"]);
            else if(i == 1)
                j.SetParticles(jinjo_particles["Jinjo_Yellow"]);
            else if (i == 2)
                j.SetParticles(jinjo_particles["Jinjo_Orange"]);
            else if (i == 3)
                j.SetParticles(jinjo_particles["Jinjo_Green"]);
            else if (i == 4)
                j.SetParticles(jinjo_particles["Jinjo_Blue"]);
            else if (i == 5)
                j.SetParticles(jinjo_particles["Jinjo_Black"]);

            jinjos.Add(j, false);
        }
    }

    public void DeleteJinjo(Jinjo j)
    {
        jinjos[j] = true;
        jinjo_to_delete = j.gameObject;
        Invoke("DeleteJinjo", 2f);
    }

    private void DeleteJinjo()
    {
        if(jinjo_to_delete != null)
        {
            jinjo_to_delete.SetActive(false);
            jinjo_to_delete = null;
        }
    }

    void Update()
    {
        if(!GameManager.Instance.Pause)
        {
            if (Time.time - _lastSpawn > spawnTime)
            {
                SpawnPlatform(Type_Platform.Classic);

                _lastSpawn = Time.time;
                if (platformCount > platformCountBeforeCheckpoint)
                {
                    CheckPointScript.Init();
                    Checkpoint.transform.position = _lastPos + new Vector3(0f, offsetYCheckpoint, 0f);
                    platformCount = 0;
                }
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
                int k = (i == 0 ? 0 : i - 1);
                Vector3 pos_pf_previous = pools[Type_Platform.Classic].usedObjects[k].transform.position;

                int j = (i + 1 < pools[Type_Platform.Classic].usedObjects.Count ? i + 1 : i);
                Vector3 pos_pf_next = pools[Type_Platform.Classic].usedObjects[j].transform.position;

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
        if(enemyManager!=null)
            return enemyManager.CheckPlatformTnt(pf, pf_previous, pf_next, offset);
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
            if (pf == null)
                return;

            Vector3 pos = _lastPos;
            pos.x += classic_width;
            pos.y += 6.66f;
            pf.SetPosition(pos);
        }
    }

    internal void SpawnPlatformTnt(EnemyManager e,Transform p)
    {
        enemyManager = e;

        Vector3 pos = _lastPos;
        pos.x += classic_width;
        enemyManager.spawnEnemy(Type_Enemy.Tnt, new Vector3(pos.x, 0, 3.0f));
    }


    public void ErasePlatform()
    {
        platformCount = 0;
        for (int i = pools[Type_Platform.Classic].usedObjects.Count-1 ; i >= 0; i--)
        {
            pools[Type_Platform.Classic].usedObjects[i].Release();
        }
        for (int i = pools[Type_Platform.Classic].unusedObjects.Count - 1; i >= 0; i--) //supprime les platforms qui ont été remises dans la liste d'unusedObjects.
        {
            pools[Type_Platform.Classic].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Platform.Classic].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Platform.Hight].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Platform.Hight].usedObjects[i].Release();
        }
        for (int i = pools[Type_Platform.Hight].unusedObjects.Count - 1; i >= 0; i--) //supprime les platforms qui ont été remises dans la liste d'unusedObjects.
        {
            pools[Type_Platform.Hight].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Platform.Hight].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }
    }

    /******* Public methods ********/

    public bool makeCurrentClassicPlatformFall()
    {
        Platform pf0 = null;
        if (getClassicPlatform(ref pf0, 2))
        {
            return pf0.makeFall(10.0f, .15f);
        }

        Platform pf = null;
        if(getClassicPlatform(ref pf, 3))
        {
            return pf.makeFall(10.0f, .3f);
        }

        Platform pf2 = null;
        if (getClassicPlatform(ref pf2, 4))
        {
            return pf2.makeFall(10.0f, .4f);
        }

        return false;
    }

    public float GetTerrainSize()
    {
        return (classic_width * 1f / spawnTime) * AudioProcessor.Instance.GetMusicLength();
    }
}
