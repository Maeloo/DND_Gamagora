using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitGame : MonoBehaviour {

	
	void Start () {
        Invoke("LaunchGame", 1.0f);
	}


    void LaunchGame()
    {
        DontDestroyOnLoad(SceneAudioManager.Instance.gameObject);
        DontDestroyOnLoad(AudioProcessor.Instance.gameObject);
        DontDestroyOnLoad(GameManager.Instance.gameObject);

        SceneManager.LoadScene("Menu");
    }
	
}
