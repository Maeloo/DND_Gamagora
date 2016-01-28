using UnityEngine;
using System.Collections;

public class LoadCharacter : Singleton<LoadCharacter>
{
    protected LoadCharacter() { }

    public bool Loaded = false;

    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        if(!Loaded)
            Init();
    }

    private void Init()
    {
        if (Character.CharacterNb == 0)
        {
            GameObject.Find("Megaman").SetActive(false);
            player = GameObject.Find("Sumo");
        }
        else
        {
            GameObject.Find("Sumo").SetActive(false);
            player = GameObject.Find("Megaman");
        }

        player.SetActive(true);

        Loaded = true;
    }

    public GameObject GetCharacter()
    {
        if(!Loaded)
        {
            Init();
        }

        return player;
    }
}
