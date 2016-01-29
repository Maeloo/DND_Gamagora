using UnityEngine;
using System.Collections;

public class LoadCharacter : Singleton<LoadCharacter>
{
    protected LoadCharacter() { }

    public bool Loaded;
    private bool first_load = true;

    private GameObject player;
    private GameObject megaman;
    private GameObject sumo;

    void Awake()
    {
        megaman = GameObject.Find("Megaman");
        sumo = GameObject.Find("Sumo");
    }

    // Use this for initialization
    void Start ()
    {
        Loaded = false;
        Init();
    }

    private void Init()
    {
        if (Character.CharacterNb == 0)
        {
            megaman.SetActive(false);
            player = sumo;
        }
        else
        {
            sumo.SetActive(false);
            player = megaman;
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
