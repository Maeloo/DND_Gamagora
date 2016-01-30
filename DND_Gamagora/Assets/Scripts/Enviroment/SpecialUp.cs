﻿using UnityEngine;
using System.Collections;

public class SpecialUp : MonoBehaviour {

    [SerializeField]
    GameObject FX;

    void OnTriggerEnter2D(Collider2D col)
    {
        Character player = col.gameObject.GetComponent<Character>();
        if (player != null)
        {
            Instantiate(FX, transform.position, Quaternion.identity);
            player.addSpcial(20.0f + 15.0f * Random.value);
            GetComponentInParent<Bonus>().Release();
        }
    }
}
