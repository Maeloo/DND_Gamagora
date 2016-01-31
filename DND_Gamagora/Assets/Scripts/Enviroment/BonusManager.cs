using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

public class BonusManager : Singleton<BonusManager>
{
    private GameObject player;

    [SerializeField]
    Bonus Note;
    [SerializeField]
    Bonus Invincibility;
    [SerializeField]
    Bonus Heart;
    [SerializeField]
    Bonus Power;
    [SerializeField]
    Bonus Special;

    protected BonusManager() {}

    private int countNote;
    private int countHeart;
    private int countPower;
    private int countSpecial;
    private int countInvincibility = 0;

    public int passCountNote = 10;
    public int passCountHeart = 5;
    public int passCountInvincibility = 10;
    public int passCountPower = 15;
    public int passCountSpecial = 15;

    private Vector3 lastBonusPos;
    private Vector3 DELTA_BONUS_CHARACTER = new Vector3(10f, 0f, 0f);
    protected Dictionary<Type_Bonus, Pool<Bonus>> pools;

    public float distMinMin = 0.1f;
    public float distMinMax = 1f;


    void Awake () {
        player = LoadCharacter.Instance.GetCharacter();
        countInvincibility = 0;
        countNote = 0;
        countHeart = 0;
        countPower = 0;
        countSpecial = 0;
        pools = new Dictionary<Type_Bonus, Pool<Bonus>>();

        Pool<Bonus> notePool = new Pool<Bonus>(Note, 8, 16);
        notePool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Note, notePool);

        Pool<Bonus> invincibilityPool = new Pool<Bonus>(Invincibility, 8, 16);
        invincibilityPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Invincibility, invincibilityPool);

        Pool<Bonus> heartsPool = new Pool<Bonus>(Heart, 8, 16);
        heartsPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Heart, heartsPool);

        Pool<Bonus> powerPool = new Pool<Bonus>(Power, 8, 16);
        powerPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Power, powerPool);

        Pool<Bonus> specialPool = new Pool<Bonus>(Special, 8, 16);
        specialPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Special, specialPool);


        lastBonusPos = player.transform.position;
    }
	
    public void SpawnBonus(Type_Bonus type)
    {
        Bonus bonus;

        Vector3 newPos = player.transform.position + DELTA_BONUS_CHARACTER;
        if (Vector3.Distance(lastBonusPos, newPos) > Random.Range(distMinMin, distMinMax))
        {
            if (type == Type_Bonus.Note)
            {
                ++countNote;
                if (passCountNote <= countNote)
                {

                    if (pools[type].GetAvailable(false, out bonus))
                    {
                        countNote = 0;
                        bonus.SetPosition(player.transform.position + DELTA_BONUS_CHARACTER);
                    }
                }

                lastBonusPos = newPos;
            }

            else if(type == Type_Bonus.Invincibility)
            {
                ++countInvincibility;
                if (passCountInvincibility <= countInvincibility)
                {
                    if (pools[type].GetAvailable(false, out bonus))
                    {
                        countInvincibility = 0;
                        bonus.SetPosition(lastBonusPos);
                    }
                }

                lastBonusPos = newPos;
            }

            else if (type == Type_Bonus.Heart)
            {
                ++countHeart;
                if (passCountHeart <= countHeart)
                {
                    if (pools[type].GetAvailable(false, out bonus))
                    {
                        countHeart = 0;
                        bonus.SetPosition(player.transform.position + DELTA_BONUS_CHARACTER);
                    }
                }

                lastBonusPos = newPos;
            }

            else if(type == Type_Bonus.Power)
            {
                ++countPower;
                if (passCountPower <= countPower)
                {
                    if (pools[type].GetAvailable(false, out bonus))
                    {
                        countPower = 0;
                        bonus.SetPosition(player.transform.position + DELTA_BONUS_CHARACTER);
                    }
                }

                lastBonusPos = newPos;
            }

            else if (type == Type_Bonus.Special)
            {
                ++countSpecial;
                if (passCountSpecial <= countSpecial)
                {
                    if (pools[type].GetAvailable(false, out bonus))
                    {
                        countSpecial = 0;
                        bonus.SetPosition(player.transform.position + DELTA_BONUS_CHARACTER);
                    }
                }

                lastBonusPos = newPos;
            }
        }
    }

    public void Respawn()
    {
        for (int i = pools[Type_Bonus.Invincibility].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Invincibility].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Invincibility].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Invincibility].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Invincibility].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Bonus.Note].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Note].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Note].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Note].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Note].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Bonus.Heart].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Heart].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Heart].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Heart].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Heart].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Bonus.Power].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Power].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Power].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Power].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Power].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Bonus.Special].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Special].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Special].unusedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Special].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Special].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }
    }
}
