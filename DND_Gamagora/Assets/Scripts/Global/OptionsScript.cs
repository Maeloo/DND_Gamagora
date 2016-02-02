using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using Game;

public class OptionsScript : Singleton<OptionsScript>
{
    protected OptionsScript() { }
    
    [SerializeField]
    private AudioMixer Mixer;

    [SerializeField]
    private float PlayerInfos_FadeIn;
    [SerializeField]
    private float PlayerInfos_FadeOut;

    private int selected_track;
    private List<AudioClip> clips;
    private Image player_sumo_btn;
    private Image player_megaman_btn;
    private float amplitude;
    private float variance;
    private float C;
    private float master_vol;
    private float music_vol;
    private float sounds_vol;
    private int player_nb;

    private Slider amplitude_slider;
    private Slider variance_slider;
    private Slider c_slider;
    private Text amplitude_text;
    private Text variance_text;
    private Text c_text;

    private Slider master_slider;
    private Slider music_slider;
    private Slider sounds_slider;
    private Text txtHP_Sumo;
    private Text txtSpeed_Sumo;
    private Text txtHP_Megaman;
    private Text txtSpeed_Megaman;
    private AudioProcessor audio_process;
    
    private AudioMixerGroup[] mixer_groups;
    private AudioMixerGroup master_mixer;
    private AudioMixerGroup music_mixer;
    private AudioMixerGroup sounds_mixer;

    private Dropdown musics;

    void Awake()
    {
        Canvas c = FindObjectOfType<Canvas>();
        audio_process = AudioProcessor.Instance;

        if (c != null)
        {
            Transform amplitude_btn = c.transform.FindChild("amplitude_button");
            Transform variance_btn = c.transform.FindChild("variance_button");
            Transform c_btn = c.transform.FindChild("c_button");
            musics = c.transform.FindChild("music_button").FindChild("DropdownMusic").GetComponent<Dropdown>();
            musics.options.Clear();
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

            txtHP_Sumo = player_sumo_btn.transform.FindChild("TextHP").GetComponent<Text>();
            txtSpeed_Sumo = player_sumo_btn.transform.FindChild("TextSpeed").GetComponent<Text>();
            txtHP_Megaman = player_megaman_btn.transform.FindChild("TextHP").GetComponent<Text>();
            txtSpeed_Megaman = player_megaman_btn.transform.FindChild("TextSpeed").GetComponent<Text>();

            txtHP_Sumo.CrossFadeAlpha(0f, 0f, true);
            txtSpeed_Sumo.CrossFadeAlpha(0f, 0f, true);
            txtHP_Megaman.CrossFadeAlpha(0f, 0f, true);
            txtSpeed_Megaman.CrossFadeAlpha(0f, 0f, true);

            Transform vol_btn = c.transform.FindChild("volume_button");
            master_slider = vol_btn.FindChild("master_button").FindChild("slider").GetComponent<Slider>();
            music_slider = vol_btn.FindChild("music_button").FindChild("slider").GetComponent<Slider>();
            sounds_slider = vol_btn.FindChild("sounds_button").FindChild("slider").GetComponent<Slider>();

            mixer_groups = Mixer.FindMatchingGroups("Master");

            foreach(AudioMixerGroup g in mixer_groups)
            {
                if (g.name.Equals("Master"))
                    master_mixer = g;
                else if(g.name.Equals("Music"))
                    music_mixer = g;
                else if (g.name.Equals("Sounds"))
                    sounds_mixer = g;
            }

            master_mixer.audioMixer.GetFloat("MasterVol", out master_vol);
            music_mixer.audioMixer.GetFloat("MusicVol", out music_vol);
            sounds_mixer.audioMixer.GetFloat("SoundsVol", out sounds_vol);

            master_slider.value = Mathf.Pow(10, (master_vol / 20f)); // Db to [0, 1]
            music_slider.value = Mathf.Pow(10, (music_vol / 20f));
            sounds_slider.value = Mathf.Pow(10, (sounds_vol / 20f));
        }
        
        clips = GameManager.Instance.Tracks;

        List<Dropdown.OptionData> datasound = new List<Dropdown.OptionData>();
        selected_track = 0;

        for (int i = 0; i < clips.Count; i++)
        {
            datasound.Add(new Dropdown.OptionData(clips[i].name));

            if (clips[i].name.Equals(audio_process.GetTrackName()))
            {
                selected_track = i;
            }   
        }

        musics.AddOptions(datasound);
        musics.value = selected_track;

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

    public void SetSelectedTrack()
    {
        selected_track = musics.value;
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

    public void ChangeMasterVolume()
    {
        master_mixer.audioMixer.SetFloat("MasterVol", 20 * Mathf.Log10(master_slider.value));
    }

    public void ChangeMusicVolume()
    {
        music_mixer.audioMixer.SetFloat("MusicVol", 20 * Mathf.Log10(music_slider.value));
    }

    public void ChangeSoundsVolume()
    {
        sounds_mixer.audioMixer.SetFloat("SoundsVol", 20 * Mathf.Log10(sounds_slider.value));
    }

    public void ShowSumoInfos()
    {
        txtHP_Sumo.CrossFadeAlpha(1f, PlayerInfos_FadeIn, true);
        txtSpeed_Sumo.CrossFadeAlpha(1f, PlayerInfos_FadeIn, true);
    }

    public void HideSumoInfos()
    {
        txtHP_Sumo.CrossFadeAlpha(0f, PlayerInfos_FadeOut, true);
        txtSpeed_Sumo.CrossFadeAlpha(0f, PlayerInfos_FadeOut, true);
    }

    public void ShowMegamanInfos()
    {
        txtHP_Megaman.CrossFadeAlpha(1f, PlayerInfos_FadeIn, true);
        txtSpeed_Megaman.CrossFadeAlpha(1f, PlayerInfos_FadeIn, true);
    }

    public void HideMegamanInfos()
    {
        txtHP_Megaman.CrossFadeAlpha(0f, PlayerInfos_FadeOut, true);
        txtSpeed_Megaman.CrossFadeAlpha(0f, PlayerInfos_FadeOut, true);
    }

    public void CancelMusicChanges()
    {
        master_mixer.audioMixer.SetFloat("MasterVol", master_vol);
        music_mixer.audioMixer.SetFloat("MusicVol", music_vol);
        sounds_mixer.audioMixer.SetFloat("SoundsVol", sounds_vol);
        GameManager.Instance.LoadScene("Menu");
    }

    public void SaveChanges()
    {
        Character.CharacterNb = player_nb;
        GameManager.Instance.SaveOptions(clips[selected_track], amplitude, variance, C);
    }

    public void play_soundsControl()
    {
        SceneAudioManager.Instance.playAudio(Audio_Type.PauseSoundsControl);
    }
}
