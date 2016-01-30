using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Game;
using UnityEngine.Audio;

public class HUDElement : MonoBehaviour
{
    Image _bar;
    Image _jinjo;

    CanvasGroup _group;

    Text _start;
    private Animator _animMusicControl;
    protected string _type;
    private Slider master_slider;
    private Slider music_slider;
    private Slider sounds_slider;

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

        if(tag.Equals("JinjoHUD"))
        {
            _jinjo = GetComponent<Image>();
        }

        if (tag.Equals("MusicControlHUD"))
        {
            _animMusicControl = GetComponent<Animator>();
            master_slider = transform.FindChild("master_button").FindChild("slider").GetComponent<Slider>();
            music_slider = transform.FindChild("music_button").FindChild("slider").GetComponent<Slider>();
            sounds_slider = transform.FindChild("sounds_button").FindChild("slider").GetComponent<Slider>();
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

    public IEnumerator startCooldown(float cooldown)
    {
        SceneAudioManager.Instance.playAudio(Audio_Type.Countdown);
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
        SceneAudioManager.Instance.playAudio(Audio_Type.Go);
    }


    public void showMusicControls(bool show)
    {
        _animMusicControl.SetBool("ShowMusicOptions", show);
    }

    public void initMusicControl(AudioMixerGroup[] mixer_groups)
    {
        float master_vol = 0f, music_vol = 0f, sounds_vol = 0f;

        foreach (AudioMixerGroup g in mixer_groups)
        {
            if (g.name.Equals("Master"))
                g.audioMixer.GetFloat("MasterVol", out master_vol);
            else if (g.name.Equals("Music"))
                g.audioMixer.GetFloat("MusicVol", out music_vol);
            else if (g.name.Equals("Sounds"))
                g.audioMixer.GetFloat("SoundsVol", out sounds_vol);
        }
        
        master_slider.value = Mathf.Pow(10, (master_vol / 20f)); // Db to [0, 1]
        music_slider.value = Mathf.Pow(10, (music_vol / 20f));
        sounds_slider.value = Mathf.Pow(10, (sounds_vol / 20f));
    }

    public float getMasterVol()
    {
        return 20 * Mathf.Log10(master_slider.value);
    }

    public float getMusicVol()
    {
        return 20 * Mathf.Log10(music_slider.value);
    }

    public float getSoundsVol()
    {
        return 20 * Mathf.Log10(sounds_slider.value);
    }
}
