using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

	public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}
