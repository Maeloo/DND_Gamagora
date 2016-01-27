using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Game;

public class HUDElement : MonoBehaviour
{

    Image _bar;

    protected string _type;

    public Type_HUD type;

    void Start ( ) {
        foreach ( Transform child in transform ) {
            if(child.CompareTag("Bar"))
            {
                _bar = child.GetComponent<Image>();
            }
        }

        HUDManager.instance.registerElement ( type, this );
    }


    public void setFillAmount(float value)
    {
        _bar.fillAmount = value;
    }
	
}
