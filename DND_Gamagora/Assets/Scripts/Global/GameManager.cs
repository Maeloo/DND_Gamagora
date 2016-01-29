using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    public List<AudioClip> Tracks { get; private set; }

    [SerializeField]
    private GameObject AudioProcess;

    public bool Pause { get; private set; }

    private SceneAudioManager audio_manager;
    private AudioProcessor audio_process;

    // Use this for initialization
    void Awake ()
    { 
        audio_manager = SceneAudioManager.Instance;
        audio_process = AudioProcessor.Instance;
        Pause = false;
        Tracks = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/"));
    }

    public void Init()
    {
        audio_process.ResetMusic();
    }

    public void reset()
    {
        LoadScene("scene");
    }

    public void SetPause(bool pause)
    {
        Pause = pause;
        if (pause)
            audio_process.PauseMusic();
        else
            audio_process.PlayMusic();
    }

    public void SetPauseHUD(bool pause)
    {
        if (pause)
        {
            HUDManager.Instance.pause(pause);
            SetPause(pause);
            Invoke("scaleTime0", 0.1f);
        }
        else
        {
            scaleTime1();
            HUDManager.Instance.pause(pause);
            SetPause(pause);
        }
    }

    private void scaleTime1()
    {
        Time.timeScale = 1f;
    }

    private void scaleTime0()
    {
        Time.timeScale = 0f;
    }

    public bool IsMainScene()
    {
        return SceneManager.GetActiveScene().name.Equals("scene");
    }

    public void StartGame()
    {
        Game.Data.ACCESSIBILITY_MODE = FindObjectOfType<Toggle>().isOn;

        LoadScene("scene");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void SaveOptions(AudioClip new_track, float new_amplitude, float new_variance, float new_c)
    {
        audio_process.ChangeTrack(new_track);
        audio_process.AmplitudeMultiplier = new_amplitude;
        audio_process.VarianceMin = new_variance;
        audio_process.C = new_c;

        LoadScene("Menu");
    }

}