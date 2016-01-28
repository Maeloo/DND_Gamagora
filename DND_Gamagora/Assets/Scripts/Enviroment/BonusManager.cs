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
    protected BonusManager() {}
    private int countNote;
    public int passCountNote = 10;
    public int passCountInvincibility = 10;
    private int countInvincibility = 0;
    protected Dictionary<Type_Bonus, Pool<Bonus>> pools;
	// Use this for initialization
	void Awake () {
        player = LoadCharacter.Instance.GetCharacter();
        countInvincibility = 0;
        countNote = 0;
        pools = new Dictionary<Type_Bonus, Pool<Bonus>>();

        Pool<Bonus> notePool = new Pool<Bonus>(Note, 8, 16);
        notePool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Note, notePool);

        Pool<Bonus> invincibilityPool = new Pool<Bonus>(Invincibility, 8, 16);
        invincibilityPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Invincibility, invincibilityPool);   
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
                    bonus.SetPosition(player.transform.position + new Vector3(10f, 0f, 0f));
                }
            }
        }
        if (type == Type_Bonus.Invincibility)
        {
            ++countInvincibility;
            if (passCountInvincibility <= countInvincibility)
            {
                if (pools[type].GetAvailable(false, out bonus))
                {
                    countInvincibility = 0;
                    bonus.SetPosition(player.transform.position + new Vector3(10f, 0f, 0f));
                }
            }
        }
    }

    public void Resapwn()
    {
        for (int i = pools[Type_Bonus.Invincibility].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Invincibility].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Invincibility].unusedObjects.Count - 1; i >= 0; i--) //supprime les platforms qui ont été remises dans la liste d'unusedObjects.
        {
            pools[Type_Bonus.Invincibility].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Invincibility].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }

        for (int i = pools[Type_Bonus.Note].usedObjects.Count - 1; i >= 0; i--)
        {
            pools[Type_Bonus.Note].usedObjects[i].Release();
        }
        for (int i = pools[Type_Bonus.Note].unusedObjects.Count - 1; i >= 0; i--) //supprime les platforms qui ont été remises dans la liste d'unusedObjects.
        {
            pools[Type_Bonus.Note].unusedObjects[i].gameObject.SetActive(false);
            pools[Type_Bonus.Note].unusedObjects[i].transform.position = new Vector3(-1000f, -1000f, -1000f);
        }
    }
}
