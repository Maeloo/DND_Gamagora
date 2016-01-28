using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsScript : Singleton<OptionsScript>
{
    protected OptionsScript() { }

    private int selected_track;
    private List<AudioClip> clips;
    private List<Button> buttons_clip;
    private Image player_sumo_btn;
    private Image player_megaman_btn;
    private float amplitude;
    private float variance;
    private float C;
    private int player_nb;

    private Slider amplitude_slider;
    private Slider variance_slider;
    private Slider c_slider;
    private Text amplitude_text;
    private Text variance_text;
    private Text c_text;
    private Transform music_panel;

    private AudioProcessor audio_process;
    private EventSystem event_sys;

    void Awake()
    {
        Canvas c = FindObjectOfType<Canvas>();
        audio_process = AudioProcessor.Instance;

        if (c != null)
        {
            Transform amplitude_btn = c.transform.FindChild("amplitude_button");
            Transform variance_btn = c.transform.FindChild("variance_button");
            Transform c_btn = c.transform.FindChild("c_button");
            music_panel = c.transform.FindChild("music_button").FindChild("music_panel");

            amplitude_slider = amplitude_btn.FindChild("amplitude_slider").GetComponent<Slider>();
            variance_slider = variance_btn.FindChild("variance_slider").GetComponent<Slider>();
            c_slider = c_btn.FindChild("c_slider").GetComponent<Slider>();
            amplitude_text = amplitude_btn.FindChild("amplitude_text").GetComponent<Text>();
            variance_text = variance_btn.FindChild("variance_text").GetComponent<Text>();
            c_text = c_btn.FindChild("c_text").GetComponent<Text>();

            amplitude = audio_process.AmplitudeMultiplier;
            variance = audio_process.VarianceMin;
            C = audio_process.C;

            amplitude_text.text = amplitude.ToString("0");
            variance_text.text = variance.ToString("0");
            c_text.text = C.ToString("0");
            amplitude_slider.value = amplitude;
            variance_slider.value = variance;
            c_slider.value = C;

            player_megaman_btn = c.transform.FindChild("player_button").FindChild("ButtonMegaman").GetComponent<Image>();
            player_sumo_btn = c.transform.FindChild("player_button").FindChild("ButtonSumo").GetComponent<Image>();
        }
        
        clips = GameManager.Instance.Tracks;

        buttons_clip = new List<Button>(clips.Count);
        buttons_clip.Add(music_panel.FindChild("Button1").GetComponent<Button>());
        buttons_clip.Add(music_panel.FindChild("Button2").GetComponent<Button>());
        buttons_clip.Add(music_panel.FindChild("Button3").GetComponent<Button>());
        buttons_clip.Add(music_panel.FindChild("Button4").GetComponent<Button>());
        
        event_sys = FindObjectOfType<EventSystem>();

        for(int i = 0; i < clips.Count; i++)
        {
            if(clips[i].name.Equals(audio_process.GetTrackName()))
                selected_track = i;

            buttons_clip[i].GetComponentInChildren<Text>().text = clips[i].name;
        }

        ColorBlock color = buttons_clip[selected_track].colors;
        color.normalColor = new Color(0.1f, 0.8f, 0f);
        buttons_clip[selected_track].colors = color;


        player_nb = Character.CharacterNb;
        if(player_nb == 0)
        {
            player_sumo_btn.color = new Color(0.1f, 0.8f, 0f);
        }
        else
        {
            player_megaman_btn.color = new Color(0.1f, 0.8f, 0f);
        }
    }

    public void SetSelectedPlayer(int nb)
    {
        if(nb != player_nb)
        {
            if (nb == 0)
            {
                player_sumo_btn.color = new Color(0.1f, 0.8f, 0f);
                player_megaman_btn.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                player_megaman_btn.color = new Color(0.1f, 0.8f, 0f);
                player_sumo_btn.color = new Color(1f, 1f, 1f, 0f);
            }

            player_nb = nb;
        }
    }

    public void SetSelectedTrack(int nb)
    {
        if(nb != selected_track)
        {
            ColorBlock c = buttons_clip[selected_track].colors;
            c.normalColor = Color.white;
            buttons_clip[selected_track].colors = c;
        }
        if (nb >= 0 && nb < clips.Count)
        {
            selected_track = nb;
            ColorBlock c = buttons_clip[nb].colors;
            c.normalColor = new Color(0.1f, 0.8f, 0f);
            buttons_clip[nb].colors = c;
            event_sys.SetSelectedGameObject(buttons_clip[nb].gameObject);
        }
    }

    public void ChangeAmplitude()
    {
        amplitude = amplitude_slider.value;
        amplitude_text.text = amplitude.ToString("0");
    }

    public void ChangeVariance()
    {
        variance = variance_slider.value;
        variance_text.text = variance.ToString("0");
    }

    public void ChangeC()
    {
        C = c_slider.value;
        c_text.text = C.ToString("0");
    }

    public void SaveChanges()
    {
        Character.CharacterNb = player_nb;
        GameManager.Instance.SaveOptions(clips[selected_track], amplitude, variance, C);
    }


}
