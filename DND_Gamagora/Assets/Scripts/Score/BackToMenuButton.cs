using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BackToMenuButton : MonoBehaviour {
    public void BackToMenu()
    {
        //DestroyImmediate(GameManager.Instance.gameObject);
        //DestroyImmediate(AudioProcessor.Instance.gameObject);
        //DestroyImmediate(SceneAudioManager.Instance.gameObject);

        //System.GC.Collect();

        SceneManager.LoadScene("Menu");
    }
	
}
