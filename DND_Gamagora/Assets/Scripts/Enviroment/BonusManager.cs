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
}
