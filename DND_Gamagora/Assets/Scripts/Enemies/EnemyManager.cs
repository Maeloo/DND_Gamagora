using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    Enemy fireball;

    [SerializeField]
    GameObject Player;

    protected EnemyManager() { }

    private Dictionary<Type_Enemy, Pool<Enemy>> pools;

    private float _lastFireball;
    private float _nextFireball;

    public List<Enemy> fireballs
    {
        get { return pools[Type_Enemy.CrazyFireball].usedObjects; }
    }


    void Awake()
    {
        pools = new Dictionary<Type_Enemy, Pool<Enemy>>();

        Pool<Enemy> poolFireballs = new Pool<Enemy>(fireball, 8, 16);
        poolFireballs.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.CrazyFireball, poolFireballs);

        _lastFireball = Time.time;
        _nextFireball = 3.0f + Random.value * 3.0f;
    }
	
    
    public bool spawnEnemy(Type_Enemy type, Vector3 position)
    {
        Enemy enemy;
        if(pools[type].GetAvailable(false, out enemy))
        {
            enemy.spawn(position);
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
    }
}
