using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

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
        DontDestroyOnLoad(AudioProcess);
        DontDestroyOnLoad(gameObject);
        Pause = false;
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

    public void StartGame()
    {
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