using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour {

    public bool local;

	void Update () {
        if (local)
            transform.localRotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.identity;
    }
    
}
