using UnityEngine;
using System.Collections;

public class TerrainManager : Singleton<TerrainManager> {


    [SerializeField] Platform ClassicPlatform;
    [SerializeField] Platform BouncyPlatform;


    protected TerrainManager ( ) { }


    private Pool<Platform> ClassicPool;
    private Pool<Platform> BouncyPool;


    void Awake()
    {
        ClassicPool = new Pool<Platform>(ClassicPlatform, 10, 20);
        BouncyPool = new Pool<Platform>(BouncyPlatform, 5, 10);
    }

}
