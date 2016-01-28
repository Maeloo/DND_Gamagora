using UnityEngine;
using UnityEngine.UI;
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

    public Toggle am;

	// Use this for initialization
	void Awake ()
    {
        audio_manager = SceneAudioManager.Instance;
        audio_process = AudioProcessor.Instance;
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
        Game.Data.ACCESSIBILITY_MODE = am.isOn;

        DontDestroyOnLoad(AudioProcess);
        DontDestroyOnLoad(audio_manager);
        DontDestroyOnLoad(gameObject);

        LoadScene("scene");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
