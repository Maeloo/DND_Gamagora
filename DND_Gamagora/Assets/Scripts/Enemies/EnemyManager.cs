﻿using UnityEngine;
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
    Enemy meteor;
    [SerializeField]
    Enemy tnt;

    private GameObject Player;

    protected EnemyManager() { }

    private Dictionary<Type_Enemy, Pool<Enemy>> pools;

    private float _lastFireball;
    private float _nextFireball;

    private float _lastShooter;
    private float _nextShooter;

    private float _lastMeteor;
    private float _nextMeteor;

    private float _lastTnt;
    private float _nextTnt;

    public List<Enemy> fireballs
    {
        get { return pools[Type_Enemy.CrazyFireball].usedObjects; }
    }

    public List<Enemy> tnts
    {
        get { return pools[Type_Enemy.Tnt].usedObjects; }
    }

    public List<Enemy> Shooters
    {
        get { return pools[Type_Enemy.Shooter].usedObjects; }
    }


    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        pools = new Dictionary<Type_Enemy, Pool<Enemy>>();

        //Init fireballs
        Pool<Enemy> poolFireballs = new Pool<Enemy>(fireball, 4, 8);
        poolFireballs.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.CrazyFireball, poolFireballs);

        _lastFireball = Time.time;
        _nextFireball = 3.0f + Random.value * 3.0f;

        //Init shooters
        Pool<Enemy> poolShooter = new Pool<Enemy>(shooter, 4, 8);
        poolShooter.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.Shooter, poolShooter);

        _lastShooter = Time.time;
        _nextShooter = 4.0f + Random.value * 4.0f;


        //Init Meteors
        Pool<Enemy> poolMeteor = new Pool<Enemy>(meteor, 4, 8);
        poolMeteor.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.Meteor, poolMeteor);

        _lastMeteor = Time.time;
        _nextMeteor = 10.0f + Random.value * 10.0f;

        //Init tnts
        Pool<Enemy> poolTnt = new Pool<Enemy>(tnt, 10, 25);
        poolTnt.automaticReuseUnavailables = true;

        pools.Add(Type_Enemy.Tnt, poolTnt);

        _lastTnt = Time.time;
        _nextTnt = 5.0f + Random.value * 5.0f;

        ScoreManager.Instance.init();
    }
	
    
    public bool spawnEnemy(Type_Enemy type, Vector3 position)
    {
        Enemy enemy;
        if(pools[type].GetAvailable(false, out enemy))
        {
            if (enemy != null)
                enemy.spawn(position, Player.transform);
        }
        return false;
    }

    public void Respawn()
    {
        for (int i = pools[Type_Enemy.CrazyFireball].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Enemy.CrazyFireball].usedObjects[i].releaseBullets();
            pools[Type_Enemy.CrazyFireball].usedObjects[i].Release();
        }

        for (int i = pools[Type_Enemy.CrazyFireball].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Enemy.CrazyFireball].unusedObjects[i].releaseBullets();
            pools[Type_Enemy.CrazyFireball].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Enemy.CrazyFireball].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);            
        }

        for (int i = pools[Type_Enemy.Shooter].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Enemy.Shooter].usedObjects[i].releaseBullets();
            pools[Type_Enemy.Shooter].usedObjects[i].Release();
        }

        for (int i = pools[Type_Enemy.Shooter].unusedObjects.Count - 1; i >= 0; i--) 
        {
            pools[Type_Enemy.Shooter].unusedObjects[i].releaseBullets();
            pools[Type_Enemy.Shooter].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Enemy.Shooter].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Enemy.Meteor].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Enemy.Meteor].usedObjects[i].Release();
        }

        for (int i = pools[Type_Enemy.Meteor].unusedObjects.Count - 1; i >= 0; i--) 
        {
            pools[Type_Enemy.Meteor].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Enemy.Meteor].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Enemy.Tnt].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Enemy.Tnt].usedObjects[i].Release();
        }

        for (int i = pools[Type_Enemy.Tnt].unusedObjects.Count - 1; i >= 0; i--) 
        {
            pools[Type_Enemy.Tnt].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Enemy.Tnt].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }
    }


    void FixedUpdate()
    {
        if(!GameManager.Instance.Pause)
        {

            if (Time.time - _lastFireball > _nextFireball)
            {
                spawnEnemy(Type_Enemy.CrazyFireball, new Vector3(Player.transform.position.x + 20.0f, 10.0f, 3.0f));

                _lastFireball = Time.time;
                _nextFireball = 3.0f + Random.value * 3.0f;
            }

            if (Time.time - _lastShooter > _nextShooter)
            {
                spawnEnemy(Type_Enemy.Shooter, new Vector3(Player.transform.position.x + 20.0f, Random.Range(-1.0f, 1.0f), 3.0f));

                _lastShooter = Time.time;
                _nextShooter = 4.0f + Random.value * 4.0f;
            }

            if (Time.time - _lastMeteor > _nextMeteor)
            {
                spawnEnemy(Type_Enemy.Meteor, new Vector3(Player.transform.position.x + 20.0f, 22.0f, 3.0f));

                _lastMeteor = Time.time;
                _nextMeteor = 10.0f + Random.value * 10.0f;
            }
        }
    }

    public bool CheckPlatformTnt(Vector3 pf, Vector3 pf_previous, Vector3 pf_next, int offset)
    {
         for (int i = 0; i < pools[Type_Enemy.Tnt].usedObjects.Count; i++)
        {
            Vector3 pos_pf = pools[Type_Enemy.Tnt].usedObjects[i].transform.position;
            if (pos_pf.x == pf.x || pos_pf.x == pf_previous.x || pos_pf.x == pf_next.x)
            {
                return true;
            }
        }
        return false;
    }
}
