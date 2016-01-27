using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Game;

public class HUDElement : MonoBehaviour
{

    Image _bar;
    Image _jinjo;
    CanvasGroup _group;

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

        _group = GetComponent<CanvasGroup>();
           
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

    public void displayGroup(bool show = true, float time = 1.0f, bool interactable = true, bool block = true)
    {
        if (_group != null)
        {
            _group.interactable = interactable;
            _group.blocksRaycasts = block;

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", show ? .0f : 1.0f,
                "to", !show ? .0f : 1.0f,
                "time", time,
                "onupdate", "changeGroupAlpha"));
        }
    }

    private void changeGroupAlpha(float value)
    {
        if (_group != null)
            _group.alpha = value;
    }

}
