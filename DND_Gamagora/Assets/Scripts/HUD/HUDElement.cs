using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Game;

public class HUDElement : MonoBehaviour
{

    Image _bar;
    Image _jinjo;
    Text _start;

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
            else if (child.CompareTag("CooldownHUD"))
                _start = child.GetComponent<Text>();
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

    public IEnumerator startCooldown(float cooldown)
    {
        float startTime = Time.time;
        float time;

        do
        {
            time = cooldown - (Time.time - startTime);
            time = time < .0f ? .0f : time;

            _start.text = time.ToString("0");

            yield return new WaitForEndOfFrame();
        } while (time > .0f);

        _start.text = "";
        HUDManager.Instance.endCooldown();
    }

}
