using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;
public class BonusManager : Singleton<BonusManager>
{
    [SerializeField]
    GameObject player;
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
        countInvincibility = 0;
        countNote = 0;
        pools = new Dictionary<Type_Bonus, Pool<Bonus>>();

        Pool<Bonus> notePool = new Pool<Bonus>(Note, 64, 128);
        notePool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Note, notePool);

        Pool<Bonus> invincibilityPool = new Pool<Bonus>(Invincibility, 8, 16);
        invincibilityPool.automaticReuseUnavailables = true;
        pools.Add(Type_Bonus.Invincibility, invincibilityPool);   
	}
	
    public void SpawnBonus(Type_Bonus type)
    {
        Bonus bonus;
        Debug.Log(type);
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

    public void RemoveBonus(Bonus obj, Type_Bonus type)
    {
        if (obj == null)
            Debug.Log("fuck"); ;

        for(int i = 0; i <  pools[type].usedObjects.Count; i++)
        {
            if(pools[type].usedObjects[i] == obj)
            {
                pools[type].usedObjects[i].Release();
                return;
            }
          //  return;
        }
        Debug.Log("Bonus not found");
        for (int i = 0; i < pools[type].unusedObjects.Count; i++)
        {
            if (pools[type].unusedObjects[i] == obj)
            {

                Debug.Log("Souci");
               // pools[type].unusedObjects[i].Release();
            }
            //  return;
        }

    }
	// Update is called once per frame
	void Update () {
	
	}
}
