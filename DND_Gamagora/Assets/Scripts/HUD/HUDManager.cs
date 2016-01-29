using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Game;

public class HUDManager : Singleton<HUDManager>
{
    protected HUDManager() { }

    Dictionary<Type_HUD, HUDElement> elements;
    private Character _player;
    private float _cooldown;
    private Canvas base_canvas;

    void Awake()
    {
        base_canvas = GetComponent<Canvas>();
    }

    public void registerElement(Type_HUD key, HUDElement element)
    {
        if (elements == null)
            elements = new Dictionary<Type_HUD, HUDElement>();

        elements.Add(key, element);

        if (key == Type_HUD.Jinjo_Violet_On ||
            key == Type_HUD.Jinjo_Yellow_On ||
             key == Type_HUD.Jinjo_Orange_On ||
              key == Type_HUD.Jinjo_Green_On ||
               key == Type_HUD.Jinjo_Blue_On ||
                key == Type_HUD.Jinjo_Black_On)
            element.SetEnable(false);

        if (key == Type_HUD.Pause)
            element.displayGroup(false, .0f, false, false);

        if (key == Type_HUD.GameOver)
            element.displayGroup(false, .0f, false, false);
    }


    public void showGameOver(bool show = true)
    {
        if(show)
        {
            pause(false);
            elements[Type_HUD.GameOver].displayGroup(true, 1.0f, true, true);
        }
        else
            elements[Type_HUD.GameOver].displayGroup(false, 0.0f, false, false);
    }


    public void setLife(float value) // [0 , 1]
    {
        elements[Type_HUD.Life].setFillAmount(value);
    }


    public void setStamina(float value) // [0 , 1]
    {
        elements[Type_HUD.Stamina].setFillAmount(value);
    }


    public void setSpecial(float value) // [0 , 1]
    {
        elements[Type_HUD.Special].setFillAmount(value);
    }

    public void setJinjo(Jinjo jinjo)
    {
        switch(jinjo.Id)
        {
            case 0:
                elements[Type_HUD.Jinjo_Violet_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Violet_Off].SetEnable(false);
                break;
            case 1:
                elements[Type_HUD.Jinjo_Yellow_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Yellow_Off].SetEnable(false);
                break;
            case 2:
                elements[Type_HUD.Jinjo_Orange_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Orange_Off].SetEnable(false);
                break;
            case 3:
                elements[Type_HUD.Jinjo_Green_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Green_Off].SetEnable(false);
                break;
            case 4:
                elements[Type_HUD.Jinjo_Blue_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Blue_Off].SetEnable(false);
                break;
            case 5:
                elements[Type_HUD.Jinjo_Black_On].SetEnable(true);
                elements[Type_HUD.Jinjo_Black_Off].SetEnable(false);
                break;
            default:
                break;
        }
    }

    public void startCooldown(Character player, float cooldown)
    {
        _player = player;
        _cooldown = cooldown;

        HUDElement elem;
        if(elements.TryGetValue(Type_HUD.Cooldown, out elem))
           StartCoroutine(elem.startCooldown(_cooldown));
    }

    public void endCooldown()
    {
        if(_player != null)
        {
            _player.EndCooldownCheckpoint();
            _player = null;
        }
    }

    private void Init()
    {
        GameManager.Instance.Init();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().Init();
    }

    public void loadScore()
    {
        
        GameManager.Instance.LoadScene("Score");
    }

    public void reset()
    {
        GameManager.Instance.LoadScene("scene");
    }

    public void quit()
    {
        //Init();
        GameManager.Instance.LoadScene("Menu");
    }

    public void pause(bool pause)
    {
        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.Pause, out elem))
        {
            elem.displayGroup(pause, 0f, pause, pause);
        }
    }

    public void resumePause()
    {
        GameManager.Instance.SetPauseHUD(false);
    }

    public void show_HUD(bool show)
    {
        base_canvas.enabled = show;
    }
}
