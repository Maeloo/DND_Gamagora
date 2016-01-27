using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Game;

public class HUDElement : MonoBehaviour
{

    Image _bar;
    Image _jinjo;

    protected string _type;

    public Type_HUD type;

    void Start ( )
    {
        foreach ( Transform child in transform )
        {
            if(child.CompareTag("Bar"))
            {
                _bar = child.GetComponent<Image>();
            }
        }

        if(tag == "JinjoHUD")
        {
            _jinjo = GetComponent<Image>();
        }
           
        HUDManager.Instance.registerElement ( type, this );
    }

    public void setFillAmount(float value)
    {
        _bar.fillAmount = value;
    }

    public void SetEnable(bool enable)
    {
        _jinjo.enabled = enable;
    }
	
}
