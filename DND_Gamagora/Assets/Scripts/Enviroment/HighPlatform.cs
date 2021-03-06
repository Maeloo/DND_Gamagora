﻿using UnityEngine;
using System.Collections;

public class HighPlatform : MonoBehaviour {

    public GameObject destruction_fx;

    protected bool destructible;


    void OnTriggerEnter2D(Collider2D col)
    {
        Bullet b = col.GetComponent<Bullet>();
        
        if (b != null && b.type == Game.Type_Bullet.Special)
        {
            GetComponentInParent<Platform>().makeFall();
            GetComponent<Rigidbody2D>().AddTorque(1000.0f);
            destructible = true;
        }

        if(b != null && b.type == Game.Type_Bullet.Player && destructible)
        {
            Hashtable param = new Hashtable();
            param.Add("volume", .4f);
            param.Add("pitch", .5f);
            SceneAudioManager.Instance.playAudio(Game.Audio_Type.Smash, param);
            Instantiate(destruction_fx, transform.position, Quaternion.identity);
            GetComponentInParent<Platform>().Release();
            destructible = false;
        }
    }
}
