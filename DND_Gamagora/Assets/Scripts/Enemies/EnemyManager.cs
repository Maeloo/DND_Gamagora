using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    Enemy fireball;
    [SerializeField]
    Enemy shooter;

    [SerializeField]
    GameObject Player;

    protected EnemyManager() { }

    private Dictionary<Type_Enemy, Pool<Enemy>> pools;

    private float _lastFireball;
    private float _nextFireball;

    private float _lastShooter;
    private float _nextShooter;

    public List<Enemy> fireballs
    {
        get { return pools[Type_Enemy.CrazyFireball].usedObjects; }
    }

    public List<Enemy> Shooters
    {
        get { return pools[Type_Enemy.Shooter].usedObjects; }
    }


    void Awake()
    {
        pools = new Dictionary<Type_Enemy, Pool<Enemy>>();

        //Init fireballs
        Pool<Enemy> poolFireballs = new Pool<Enemy>(fireball, 8, 16);
        poolFireballs.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.CrazyFireball, poolFireballs);

        _lastFireball = Time.time;
        _nextFireball = 3.0f + Random.value * 3.0f;

        //Init shooters
        Pool<Enemy> poolShooter = new Pool<Enemy>(shooter, 8, 16);
        poolShooter.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.Shooter, poolShooter);

        _lastShooter = Time.time;
        _nextShooter = 4.0f + Random.value * 4.0f;
    }
	
    
    public bool spawnEnemy(Type_Enemy type, Vector3 position)
    {
        Enemy enemy;
        if(pools[type].GetAvailable(false, out enemy))
        {
            enemy.spawn(position, Player.transform);
        }
        return false;
    }


    void FixedUpdate()
    {
        if(Time.time - _lastFireball > _nextFireball)
        {
            spawnEnemy(Type_Enemy.CrazyFireball, new Vector3(Player.transform.position.x + 20.0f, .0f, 3.0f));

            _lastFireball = Time.time;
            _nextFireball = 3.0f + Random.value * 3.0f;
        }

        if (Time.time - _lastShooter > _nextShooter)
        {
            spawnEnemy(Type_Enemy.Shooter, new Vector3(Player.transform.position.x + 20.0f, Random.Range(-1.0f, 1.0f), 3.0f));

            _lastShooter = Time.time;
            _nextShooter = 4.0f + Random.value * 4.0f;
        }
    }
}
