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

    protected BonusManager() {}

    private int countNote;
    private int countHeart;
    private int countPower;
    private int countInvincibility = 0;

    public int passCountNote = 10;
    public int passCountHeart = 5;
    public int passCountInvincibility = 10;
    public int passCountPower = 15;

    private Vector3 lastBonusPos;
    private Vector3 DELTA_BONUS_CHARACTER = new Vector3(10f, 0f, 0f);
    protected Dictionary<Type_Bonus, Pool<Bonus>> pools;

    public float distMinInvulnerability = 10f;
    

    void Awake () {
        lastBonusPos = new Vector3(0f, 0f, 0f);
        player = LoadCharacter.Instance.GetCharacter();
        countInvincibility = 0;
        countNote = 0;
        countHeart = 0;
        countPower = 0;
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
        heartsPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Power, powerPool);
    }
	
    public void SpawnBonus(Type_Bonus type)
    {
        Bonus bonus;
        
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
        }

        if (type == Type_Bonus.Invincibility)
        {
            ++countInvincibility;
            Vector3 newPos = player.transform.position + DELTA_BONUS_CHARACTER;
            if (passCountInvincibility <= countInvincibility && Vector3.Distance(lastBonusPos, newPos) > distMinInvulnerability)
            {
                if (pools[type].GetAvailable(false, out bonus))
                {
                    countInvincibility = 0;
                    lastBonusPos = newPos;
                    bonus.SetPosition(lastBonusPos);
                }
            }
        }

        if (type == Type_Bonus.Heart)
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
        }

        if(type == Type_Bonus.Power)
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
    }
}
