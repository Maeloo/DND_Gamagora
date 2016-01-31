using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine.Audio;

public class HUDManager : Singleton<HUDManager>
{
    protected HUDManager() { }

    [SerializeField]
    private AudioMixer Mixer;

    Dictionary<Type_HUD, HUDElement> elements;
    private Character _player;
    private float _cooldown;
    private Canvas base_canvas;

    private bool show_music_controls;
    private bool music_controls_showed;
    private AudioMixerGroup[] mixer_groups;
    private static int pause_sound = -1;

    void Awake()
    {
        base_canvas = GetComponent<Canvas>();
        show_music_controls = false;
        music_controls_showed = false;
        mixer_groups = Mixer.FindMatchingGroups("Master");
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

        if (key == Type_HUD.End)
            element.displayGroup(false, .0f, false, false);

        if (key == Type_HUD.MusicControl)
            element.initMusicControl(mixer_groups);
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


    public void showEnd(bool show = true)
    {
        if (show)
        {
            pause(false);
            elements[Type_HUD.End].displayGroup(true, 1.0f, true, true);
        }
        else
            elements[Type_HUD.End].displayGroup(false, 0.0f, false, false);
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
        reset_MusicOptions();

        if(!pause)
            GameManager.Instance.StopMenuMusic();

        if(pause)
        {
            if (pause_sound != -1)
                SceneAudioManager.Instance.stop(pause_sound);
            Hashtable param = new Hashtable();
            param.Add("starttime", 0.1f);
            pause_sound = SceneAudioManager.Instance.playAudio(Audio_Type.Pause, param);
        }

        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.Pause, out elem))
        {
            elem.displayGroup(pause, 0f, pause, pause);
        }

        music_controls_showed = false;
    }

    public void resumePause()
    {
        GameManager.Instance.SetPauseHUD(false);
    }

    public void show_HUD(bool show)
    {
        base_canvas.enabled = show;
    }

    public void show_MusicOptions()
    {
        show_music_controls = !show_music_controls;

        if (show_music_controls)
            GameManager.Instance.PlayMenuMusic();
        else
            GameManager.Instance.PauseMenuMusic();

        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.MusicControl, out elem))
        {
            if (show_music_controls && !music_controls_showed)
            {
                music_controls_showed = true;
            }

            elem.showMusicControls(show_music_controls);
        }
    }

    private void reset_MusicOptions()
    {
        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.MusicControl, out elem))
        {
            elem.showMusicControls(false);
        }
    }

    public void ChangeMasterVolume()
    {
        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.MusicControl, out elem))
        {
            elem.setMasterVol();
        }
    }

    public void ChangeMusicVolume()
    {
        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.MusicControl, out elem))
        {
            elem.setMusicVol();
        }
    }

    public void ChangeSoundsVolume()
    {
        HUDElement elem;

        if (elements.TryGetValue(Type_HUD.MusicControl, out elem))
        {
            elem.setSoundsVol();
        }
    }
}
